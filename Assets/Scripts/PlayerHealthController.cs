using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    public static PlayerHealthController instance;

    public int maxHealth, currentHealth;
    

    public void Awake()
    {
        instance=this;
    }
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        UIController.instance.healthSlider.maxValue = maxHealth;
        UIController.instance.healthSlider.value = currentHealth;
        UIController.instance.healthText.text = "Health: " + currentHealth + "/" + maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DamagePlayer(int damageAmount)
    {
        currentHealth -= damageAmount;
        UIController.instance.showDamage();
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            gameObject.SetActive(false);
            AudioManager.instance.stopBGM();
            UIController.instance.healthSlider.value = currentHealth;
            UIController.instance.healthText.text = "Health: " + currentHealth + "/" + maxHealth;
        }
        UIController.instance.healthSlider.value = currentHealth;
        UIController.instance.healthText.text = "Health: " + currentHealth + "/" + maxHealth;

        
        
    }
}
