using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    public int damageAmount = 1;
    public string wallTag = "Wall";
    public string enemyTag = "Enemy";

    private void OnTriggerEnter2D(Collider2D other)
    {
        MeleEnemyAI meleEnemy = other.GetComponent<MeleEnemyAI>();
        RangeEnemyAI rangeEnemy = other.GetComponent<RangeEnemyAI>();   

        if (meleEnemy != null)
        {
            meleEnemy.TakeDamage(damageAmount);
            Destroy(gameObject);
        }

        if (rangeEnemy != null)
        {
            rangeEnemy.TakeDamage(damageAmount);
            Destroy(gameObject);
        }

        if (other.CompareTag(wallTag))
        {
            Destroy(gameObject);
        }
    }

}
