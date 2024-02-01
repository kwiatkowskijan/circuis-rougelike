using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreseSpeedPickup : MonoBehaviour
{
    public float increseSpeedAmount = 1f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerBehavior player = other.GetComponent<PlayerBehavior>();

            if (player != null)
            {
                player.IncreseSpeed(increseSpeedAmount);
                Destroy(gameObject);
            }
        }
    }
}
