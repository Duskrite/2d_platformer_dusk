using UnityEngine;

public class Enemybullet : MonoBehaviour
{
    public float lifeTime = 5f;
    public GameObject afterVFX;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 3 || collision.gameObject.layer == 6)
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (afterVFX != null)
        {
            Instantiate(afterVFX, transform.position, Quaternion.identity);
        }
    }
}