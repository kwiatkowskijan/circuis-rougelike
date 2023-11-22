using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    public int damageAmount = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        EnemyAI enemy = other.GetComponent<EnemyAI>();

        if (enemy != null)
        {
            enemy.TakeDamage(damageAmount);
            Destroy(gameObject);
        }

        if (gameObject.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }

}
