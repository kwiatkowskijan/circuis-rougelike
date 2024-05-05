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
    public Sprite bodyRight;
    public Sprite headRight;
    public Sprite headLeft;
    public Sprite headUp;
    public Sprite headDown;

    public int maxHealth = 3;
    private int currentHealth;
    public HeartDisplay heartDisplay;

    private void Start()
    {
        currentHealth = maxHealth;
        heartDisplay.UpdateHearts(currentHealth);
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
        
        if(movement.x != 0 || movement.y != 0)
        {
            Transform body = transform.Find("Body");

            if(body != null)
            {
                Animator animator = body.GetComponent<Animator>();
                animator.SetBool("isMoving", true);
            }
        }
        else
        {
            Transform body = transform.Find("Body");

            if (body != null)
            {
                Animator animator = body.GetComponent<Animator>();
                animator.SetBool("isMoving", false);
            }
        }

        shootDir.x = Input.GetAxis("HorizontalShoot");
        shootDir.y = Input.GetAxis("VerticalShoot");

        if((shootDir.x != 0  || shootDir.y != 0) && Time.time > lastFire + fireDelay)
        {
            Shoot(shootDir.x, shootDir.y);
            lastFire = Time.time;
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Transform head = transform.Find("Head");

            SpriteRenderer headSprite = head.GetComponent<SpriteRenderer>();

            if (head != null)
            {
                headSprite.sprite = headRight;
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Transform head = transform.Find("Head");

            SpriteRenderer headSprite = head.GetComponent<SpriteRenderer>();

            if (head != null)
            {
                headSprite.sprite = headLeft;
            }
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Transform head = transform.Find("Head");

            SpriteRenderer headSprite = head.GetComponent<SpriteRenderer>();

            if (head != null)
            {
                headSprite.sprite = headUp;
            }
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Transform head = transform.Find("Head");

            SpriteRenderer headSprite = head.GetComponent<SpriteRenderer>();

            if (head != null)
            {
                headSprite.sprite = headDown;
            }
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

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        heartDisplay.UpdateHearts(currentHealth);
    }

}
