using UnityEngine;

public class MovingElement : MonoBehaviour
{
    public Transform pointA, pointB;
    public float speed = 2.0f;
    public bool isPlatform;

    private Vector3 targetPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        targetPosition = pointA.position;

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        if (transform.position == targetPosition)
        {
            targetPosition = (targetPosition == pointA.position) ? pointB.position : pointA.position;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isPlatform && collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (isPlatform && collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }
}
