using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    public int damageAmount = 1;
    public string wallTag = "Wall";
    public string enemyTag = "Enemy";
    [SerializeField] private ParticleSystem BulletDestroy;
    [SerializeField] private ParticleSystem BulletHit;

    private void OnTriggerEnter2D(Collider2D other)
    {
        MeleEnemyAI meleEnemy = other.GetComponent<MeleEnemyAI>();
        RangeEnemyAI rangeEnemy = other.GetComponent<RangeEnemyAI>();

        if (meleEnemy != null)
        {
            Instantiate(BulletHit, transform.position, Quaternion.identity);
            meleEnemy.TakeDamage(damageAmount);
            Destroy(gameObject);
        }

        if (rangeEnemy != null)
        {
            Instantiate(BulletHit, transform.position, Quaternion.identity);
            rangeEnemy.TakeDamage(damageAmount);
            Destroy(gameObject);
        }

        if (other.CompareTag(wallTag) || other.CompareTag("FirstRoomWalls"))
        {
            Instantiate(BulletDestroy, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
