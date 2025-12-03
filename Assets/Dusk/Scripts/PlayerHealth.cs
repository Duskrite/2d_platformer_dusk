using System.Collections;
using JetBrains.Annotations;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3;
    int currentHealth;
    bool isInvincible = false;

    public SpriteRenderer sr;
    public HeartUI healthUI;
    public Color InvincibleColor;

    public InGameUIController inGameUIController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
        healthUI.SetHeartUI(maxHealth);
        sr = GetComponent<SpriteRenderer>(); 

        HeartItem.OnHeartItemCollected += Heal;
    }

    public void Heal(int healthAmount)
    {
        currentHealth += healthAmount;

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        healthUI.UpdateHeartUI(currentHealth);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IItem item = collision.gameObject.GetComponent<IItem>();

        if (item != null)
        {
            item.Collect();
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isInvincible)
        {
            BaseEnemyAI enemyAI = collision.gameObject.GetComponent<BaseEnemyAI>();
            WayPointEnemy wayPointEnemy = collision.gameObject.GetComponent<WayPointEnemy>();
            Trap trap = collision.gameObject.GetComponent<Trap>();

            if (enemyAI)
            {
                TakeDamage(enemyAI.damage);
                StartCoroutine(Invincibility());
            }

            if (wayPointEnemy)
            {
                TakeDamage(wayPointEnemy.damage);
                StartCoroutine(Invincibility());
            }

            if (trap && trap.damage > 0)
            {
                TakeDamage(trap.damage);
                StartCoroutine(Invincibility());
            }
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            inGameUIController.Defeat();
        }

        healthUI.UpdateHeartUI(currentHealth);
    }

    public IEnumerator Invincibility()
    {
        isInvincible = true;

        sr.color = InvincibleColor;
        yield return new WaitForSeconds(0.2f);
        sr.color = Color.white;
        yield return new WaitForSeconds(0.2f);
        sr.color = InvincibleColor;
        yield return new WaitForSeconds(0.2f);
        sr.color = Color.white;
        yield return new WaitForSeconds(0.2f);
        sr.color = InvincibleColor;
        yield return new WaitForSeconds(0.2f);
        sr.color = Color.white;

        isInvincible = false;
    }
}
