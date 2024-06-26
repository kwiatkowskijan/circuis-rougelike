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
    public GameObject currentRoom;
    public DungeonGenerator dungeonGenerator;
    [SerializeField] private ParticleSystem DeathParticles;
    private Animator bodyAnimator;

    public void Start()
    {
        currentHealth = maxHealth;

        Transform childTransform = transform.Find("Body");

        if (childTransform != null)
        {
            bodyAnimator = childTransform.GetComponent<Animator>();
        }
        else
        {
            Debug.LogError("Error");
        }
    }
    private void Update()
    {
        if (player != null)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            transform.Translate(direction * moveSpeed * Time.deltaTime);

            bodyAnimator.SetBool("IsWalking", true);

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
        else
        {
            bodyAnimator.SetBool("IsWalking", false);
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
        Instantiate(DeathParticles, transform.position, Quaternion.identity);

        Destroy(this.gameObject);

        if (currentRoom != null && dungeonGenerator != null)
        {
            dungeonGenerator.DecreaseEnemiesInRoom(currentRoom);
        }
    }

    void Shoot()
    {
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        ProjectileScript projectileScript = projectile.GetComponent<ProjectileScript>();
        if (projectileScript != null)
        {
            projectileScript.SetDirection((player.position - transform.position).normalized);
        }
    }

}
