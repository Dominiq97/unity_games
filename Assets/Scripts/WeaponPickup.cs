using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    // Start is called before the first frame update
    public string theGun;
    private bool collected;
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !collected)
        {
            //give ammo
            PlayerController.instance.addGun(theGun);
            Destroy(gameObject);
            collected = true;
        }
    }
}
