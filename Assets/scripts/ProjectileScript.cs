using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    public float speed = 7.5f;
    private Vector3 direction;
    public float lifeTime = 3.0f;

    public void Start()
    {
        StartCoroutine(DeathDelay());
    }
    public void SetDirection(Vector3 dir)
    {
        direction = dir;
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerLifeSystem playerLife = other.GetComponent<PlayerLifeSystem>();
            if (playerLife != null)
            {
                playerLife.TakeDamage(1);
            }

            Destroy(gameObject);
        }
    }

    IEnumerator DeathDelay()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }
}
