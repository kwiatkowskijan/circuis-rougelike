using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    public float speed = 7.5f;
    private Vector3 direction;
    public float lifeTime = 1.0f;
    [SerializeField] private ParticleSystem BulletDestroy;
    [SerializeField] private ParticleSystem BulletHit;

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
            PlayerBehavior player = other.GetComponent<PlayerBehavior>();

            if (player != null)
            {
                Instantiate(BulletHit, transform.position, Quaternion.identity);
                player.TakeDamage(1);
            }

            Destroy(gameObject);
        }

        if (other.CompareTag("Wall") || other.CompareTag("FirstRoomWalls"))
        {
            Instantiate(BulletDestroy, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

    }

    IEnumerator DeathDelay()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }
}
