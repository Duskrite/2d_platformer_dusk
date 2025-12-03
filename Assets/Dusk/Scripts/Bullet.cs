using JetBrains.Annotations;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int BulletDamage = 1;
    public int lifeTime = 4;
    public GameObject afterVFX;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        BaseEnemyAI baseEnemyAI = collision.GetComponent<BaseEnemyAI>();

        if (baseEnemyAI)
        {
            // Apply damage to the enemy
            baseEnemyAI.TakeDamage(BulletDamage);
            // Destroy the bullet after hitting the enemy
            Destroy(gameObject);
        }

        WayPointEnemy wayPointEnemy = collision.GetComponent<WayPointEnemy>();

        if (wayPointEnemy)
        {
            // Apply damage to the enemy
            wayPointEnemy.TakeDamage(BulletDamage);
            // Destroy the bullet after hitting the enemy
            Destroy(gameObject);
        }

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
