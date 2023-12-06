using UnityEngine.SceneManagement;
using UnityEngine;

public class PlayerLifeSystem : MonoBehaviour
{
    public int maxHealth = 3;
    public int currentHealth;

    public HeartDisplay heartDisplay;

    public void Start()
    {
        currentHealth = maxHealth;
        heartDisplay.UpdateHearts(currentHealth);
    }

    public void Update()
    {
        //Debug.Log("Max zdrowie: " + maxHealth);
        //Debug.Log("Aktualne zdrowie: " + currentHealth);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            SceneManager.LoadScene("SampleScene");
        }
        else
        {
            heartDisplay.UpdateHearts(currentHealth);
        }
    }

    public void IncreaseMaxHealth(int MaxHealthAmmount)
    {
        maxHealth = maxHealth + MaxHealthAmmount;
        currentHealth = maxHealth;
    }
}
