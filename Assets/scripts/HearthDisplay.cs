using UnityEngine;
using UnityEngine.UI;

public class HeartDisplay : MonoBehaviour
{
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    private PlayerLifeSystem playerLifeSystem;

    void Start()
    {
        playerLifeSystem = FindObjectOfType<PlayerLifeSystem>();

        if (playerLifeSystem == null)
        {
            Debug.LogError("Nie mo¿na znaleŸæ obiektu PlayerLifeSystem.");
        }
        else
        {
            UpdateHearts(playerLifeSystem.maxHealth);
        }
    }

    public void UpdateHearts(int currentHealth)
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < currentHealth)
                hearts[i].sprite = fullHeart;
            else
                hearts[i].sprite = emptyHeart;
        }
    }
}
