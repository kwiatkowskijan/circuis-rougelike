using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleEnemyAI : MonoBehaviour
{
    public Transform player;
    public float moveSpeed = 3.0f;

    public int maxHealth = 5;
    public int currentHealth;

    public float knockbackForce = 5f;

    public void Start()
    {
        currentHealth = maxHealth;
    }
    private void Update()
    {
        if (player != null)
        {
            Vector3 direction = (player.position - transform.position).normalized;

            transform.Translate(direction * moveSpeed * Time.deltaTime);
        }

    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Destroy(this.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerLifeSystem playerLifeSystem = collision.gameObject.GetComponent<PlayerLifeSystem>();
            if (playerLifeSystem != null)
            {
                playerLifeSystem.TakeDamage(1);
            }

        }
    }
}
