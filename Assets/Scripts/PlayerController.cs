using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public static PlayerController instance;

    public float moveSpeed, gravityModifier, jumpPower, runSpeed=12f;
    public CharacterController charCon;

    private Vector3 moveInput;

    public Transform camTrans;

    public float mouseSensitivity;
    public bool invertX;
    public bool invertY;

    private bool canJump, canDoubleJump;
    public Transform groundCheckPoint;
    public LayerMask whatIsGround = LayerMask.GetMask("Ground");

    public List<Gun> unlockableGuns = new List<Gun>();

    public Animator anim;
    
    public Transform firePoint;

    public Gun activeGun;
    public List<Gun> guns = new List<Gun>();
    public int currentGun;

    // Start is called before the first frame update


    private void Awake()
    {
        instance = this;
    }


    void Start()
    {
        currentGun--;
        SwitchGun();
    }
    // Update is called once per frame
    void Update()
    {
        //moveInput.x = Input.GetAxis("Horizontal")*moveSpeed* Time.deltaTime;
        //moveInput.z = Input.GetAxis("Vertical")*moveSpeed*Time.deltaTime;
        if (!UIController.instance.pauseSceen.activeInHierarchy)
        {

        

            //store y velocity

            float yStore = moveInput.y;

            Vector3 vertMove = transform.forward * Input.GetAxis("Vertical");
            Vector3 horiMove = transform.right * Input.GetAxis("Horizontal");

            moveInput = horiMove + vertMove;
            moveInput.Normalize();

            if (Input.GetKey(KeyCode.LeftShift)){
                moveInput = moveInput * runSpeed;
            }
            else { 
            moveInput = moveInput * moveSpeed;
            }
            moveInput.y = yStore;

            moveInput.y +=Physics.gravity.y * gravityModifier*Time.deltaTime;

            if (charCon.isGrounded)
            {
                moveInput.y = Physics.gravity.y*gravityModifier*Time.deltaTime;
            }


            canJump = Physics.OverlapSphere(groundCheckPoint.position, 1f, whatIsGround).Length > 0;
            //jumping
            if (canJump)
            {
                canDoubleJump = false;
            }

            if ((Input.GetKeyDown(KeyCode.Space)))
            {
                moveInput.y += jumpPower;
                canDoubleJump = true;
            }
            else if (canDoubleJump && (Input.GetKeyDown(KeyCode.Space)))
            {
                moveInput.y = jumpPower;
                canDoubleJump = false;
            }

            charCon.Move(moveInput * Time.deltaTime);

            // control camera rotation
            Vector2 mouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"))*mouseSensitivity;

            if (invertX) {
                mouseInput.x = -mouseInput.x;
            }
            if (invertY)
            {
                mouseInput.y = -mouseInput.y;
            }

            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y+mouseInput.x, transform.rotation.eulerAngles.z);

            camTrans.rotation = Quaternion.Euler(camTrans.rotation.eulerAngles + new Vector3(-mouseInput.y, 0f, 0f));

            //handle Shooting
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                if (Physics.Raycast(camTrans.position, camTrans.forward, out hit, 50f))
                {
                    if (Vector3.Distance(camTrans.position, hit.point) > 2f)
                    {
                        firePoint.LookAt(hit.point);
                    }
                
                }else
                {
                    firePoint.LookAt(camTrans.position + (camTrans.forward * 30f));
                }

                //Instantiate(bullet, firePoint.position, firePoint.rotation);
                FireShot();
            }

            if (Input.GetMouseButton(0) && activeGun.canAutoFire)
            {
                if (activeGun.fireCounter <= 0)
                {
                    FireShot();
                }
            }

            if (Input.GetKeyDown(KeyCode.F)){
                SwitchGun();
            }


            anim.SetFloat("moveSpeed", moveInput.magnitude);
            anim.SetBool("onGround", canJump);
        }
    }


    public void FireShot()
    {
        if (activeGun.currentAmmo > 0) { 
            activeGun.currentAmmo -= 1;
            Instantiate(activeGun.bullet, firePoint.position, firePoint.rotation);
            activeGun.fireCounter = activeGun.fireRate;
            UIController.instance.ammoText.text = "Ammo: " + activeGun.currentAmmo;
        }
    }

    public void SwitchGun()
    {
        activeGun.gameObject.SetActive(false);

        currentGun++;
        if (currentGun>=guns.Count) {
            currentGun = 0;
        }

        activeGun = guns[currentGun];
        activeGun.gameObject.SetActive(true);
        UIController.instance.ammoText.text = "Ammo: " + activeGun.currentAmmo;
    }


    public void addGun(string gunToAdd)
    {
        bool gunUnlocked = false;
        if (unlockableGuns.Count > 0)
        {
            for(int i = 0; i < unlockableGuns.Count; i++)
            {
                if (unlockableGuns[i].gunName == gunToAdd)
                {
                    gunUnlocked = true;
                    guns.Add(unlockableGuns[i]);

                    unlockableGuns.RemoveAt(i);
                    i = unlockableGuns.Count;
                }
            }
        }
        if (gunUnlocked)
        {
            currentGun = guns.Count - 2;
            SwitchGun();
        }
    }
}
