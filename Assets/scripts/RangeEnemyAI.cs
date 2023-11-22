using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeEnemyAI : MonoBehaviour
{
    public Transform player;
    public float moveSpeed = 3.0f;

    public int maxHealth = 5;
    public int currentHealth;

    public GameObject projectilePrefab;
    public Transform firePoint;
    public float timeBetweenShots = 0.5f;
    private float shotCounter;

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

            if (shotCounter <= 0)
            {
                Shoot();
                shotCounter = timeBetweenShots;
            }
            else
            {
                shotCounter -= Time.deltaTime;
            }
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

    void Shoot()
    {
        // Tworzenie pocisku na podstawie prefabrykatu
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        // Ustawianie kierunku lotu pocisku
        ProjectileScript projectileScript = projectile.GetComponent<ProjectileScript>();
        if (projectileScript != null)
        {
            projectileScript.SetDirection((player.position - transform.position).normalized);
        }
    }

}
