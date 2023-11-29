using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float speed = 5f;
    public Rigidbody2D rb;
    public Vector2 movement;

    public GameObject bulletPrefab;
    public float bulletSpeed;
    private float lastFire;
    public float fireDelay;
    public Vector2 shootDir;

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime); 

        shootDir.x = Input.GetAxis("HorizontalShoot");
        shootDir.y = Input.GetAxis("VerticalShoot");

        if((shootDir.x != 0  || shootDir.y != 0) && Time.time > lastFire + fireDelay)
        {
            Shoot(shootDir.x, shootDir.y);
            lastFire = Time.time;
        }

    }

    void Shoot(float x, float y)
    {
        GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation) as GameObject;
        bullet.AddComponent<Rigidbody2D>().gravityScale = 0;
        Vector2 shootDirection = new Vector2(x, y).normalized;
        float angle = Mathf.Atan2(shootDirection.normalized.y, shootDirection.normalized.x) * Mathf.Rad2Deg;
        bullet.transform.rotation = Quaternion.Euler(0, 0, angle);
        bullet.GetComponent<Rigidbody2D>().velocity = shootDirection * bulletSpeed;
    }

}
