using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EnemyHealthController : MonoBehaviour
{

    public int currentHealth = 5;
    public int maxHealth;

    public Slider enemyHealthSlider;
    public GameObject healthBarUI;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        enemyHealthSlider.value = CalculateHealth();
    }

    // Update is called once per frame
    void Update()
    {
       // enemyHealthSlider.value = CalculateHealth();
    }
    public void DamageEnemy(int damageAmount)
    {
        currentHealth -=damageAmount;
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }

        enemyHealthSlider.value = CalculateHealth()+50;
        if (currentHealth < maxHealth)
        {
            healthBarUI.SetActive(true);
        }
        healthBarUI.SetActive(true);
    }
    float CalculateHealth()
    {
        return currentHealth / maxHealth*100;
    }
}
