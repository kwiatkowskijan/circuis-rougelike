using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseMaxHealth : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerLifeSystem playerLife = other.GetComponent<PlayerLifeSystem>();
            if (playerLife != null)
            {
                playerLife.IncreaseMaxHealth(1);
            }

            Destroy(gameObject);
        }
    }
}
