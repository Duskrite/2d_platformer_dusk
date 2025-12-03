using UnityEngine;

public class Trap : MonoBehaviour
{
    public int damage = 1;
    public float bounceForce = 5f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            HandleBounceForce(collision.gameObject);
        }
    }

    private void HandleBounceForce(GameObject gb)
    {
        Rigidbody2D rb = gb.GetComponent<Rigidbody2D>();

        if (rb)
        {
            rb.linearVelocityY = 0;
            rb.AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse);
        }
    }
}
