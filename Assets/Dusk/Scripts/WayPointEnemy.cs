using UnityEngine;
using UnityEngine.Rendering;
using System.Collections;


public class WayPointEnemy : MonoBehaviour
{
    public float speed = 5;
    public int damage = 1;
    public int maxHealth = 3;
    public int currentHealth;
    SpriteRenderer spriteRenderer;
    Color ogColor;
    public Transform pointA, pointB, currentPoint;
    public Rigidbody2D rb;
    //public Animator anim;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        //anim = GetComponent<Animator>();
        currentPoint = pointA;
        currentHealth = maxHealth;
        ogColor = spriteRenderer.color;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 point = transform.position - currentPoint.position;

        if (currentPoint == pointB)
        {
            rb.linearVelocity = new Vector2(speed, 0);
        }
        else
        {
            rb.linearVelocity = new Vector2(-speed, 0);
        }

        if (Vector2.Distance(transform.position, pointB.position) < 0.5f && currentPoint == pointB)
        {
            currentPoint = pointA;
            Flip();
        }
        else if (Vector2.Distance(transform.position, pointA.position) < 0.5f && currentPoint == pointA)
        {
            currentPoint = pointB;
            Flip();
        }
    }

    public void Flip()
    {
        Vector3 ls = transform.localScale;
        ls.x *= -1;
        transform.localScale = ls;
    }


    public void TakeDamage(int bulletDamage)
    {
        currentHealth -= bulletDamage;

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(FlashRed());
        }
    }

    public IEnumerator FlashRed()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = ogColor;
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
