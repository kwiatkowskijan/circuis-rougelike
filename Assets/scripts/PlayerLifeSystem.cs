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

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        heartDisplay.UpdateHearts(currentHealth);
    }
}
