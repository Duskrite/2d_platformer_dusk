using System.Collections;
using Pathfinding;
using UnityEngine;

[RequireComponent(typeof(Seeker))]
[RequireComponent(typeof(Rigidbody2D))]
public class BaseEnemyAI : MonoBehaviour
{
    [Header("Target")]
    public Transform player, guard;

    [Header("Timer & Distance")]
    public float chaseDistance = 5f;
    public float susDuration = 3f;
    public float lastSawPlayerTimer = 0f;

    [Header("Movement")]
    public float speed = 10f;
    public bool isFacingRight;

    [Header("PathFinding")]
    public float wayPointThreshold = 0.5f;
    public float pathUpdateInterval = 0.5f;

    public int damage = 1;
    public int maxHealth = 3;
    public int currentHealth;
    SpriteRenderer spriteRenderer;
    Color ogColor;

    protected Rigidbody2D rb; // Only available to this class and derived classes
    protected Path path;
    protected int currentWaypoint = 0;
    Seeker seeker;
    public bool isChasing;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        seeker = GetComponent<Seeker>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = GameObject.FindWithTag("Player").transform;

        InvokeRepeating(nameof(UpdatePath), 0f, pathUpdateInterval);

        currentHealth = maxHealth;
        ogColor = spriteRenderer.color;
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer < chaseDistance)
        {
            isChasing = true;
            lastSawPlayerTimer = 0f;
        }
        else if (isChasing)
        {
            lastSawPlayerTimer += Time.deltaTime;

            if (lastSawPlayerTimer >= susDuration)
            {
                isChasing = false;
            }
        }
    }

    private void FixedUpdate()
    {
        if (path == null || path.vectorPath == null || path.vectorPath.Count == 0) return;


        Vector2 wayPoint = path.vectorPath[currentWaypoint];

        Vector2 direction = wayPoint - rb.position;

        Move(direction);

        // Advance to the next waypoint if close enough
        AdvanceWayPoint();
    }

    public void UpdatePath()
    {
        if (seeker.IsDone())
        {
            Vector2 target;

            if (isChasing)
            {
                target = player.position;
            }
            else
            {
                target = guard.position;
            }

            seeker.StartPath(rb.position, target, OnPathComplete);
        }
    }

    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    public void AdvanceWayPoint()
    {
        if (Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]) < wayPointThreshold)
        {
            currentWaypoint = Mathf.Min(currentWaypoint + 1, path.vectorPath.Count - 1);
        }
    }

    public virtual void Move(Vector2 direction)
    {
        
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseDistance);
    }
}