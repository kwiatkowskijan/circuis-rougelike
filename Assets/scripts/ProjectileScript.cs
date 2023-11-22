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
        // Sprawdzenie kolizji z graczem
        if (other.CompareTag("Player"))
        {
            // Pobranie komponentu PlayerLifeSystem z obiektu gracza
            PlayerLifeSystem playerLife = other.GetComponent<PlayerLifeSystem>();
            if (playerLife != null)
            {
                // Odejmowanie ¿ycia gracza
                playerLife.TakeDamage(1);
            }

            // Zniszczenie pocisku po trafieniu w gracza
            Destroy(gameObject);
        }
    }

    IEnumerator DeathDelay()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }
}
