using UnityEngine;
using System.Collections;

public class GroundEnemyAi2 : BaseEnemyAI
{
    [Header("Jumping")]
    public float jumpForce = 10f;
    public float jumpCooldown = 1f;
    public float jumpThreshold = 1f;
    public bool canJump = true;

    [Header("Ground Check")]
    public Transform groundCheckPosition;
    public float groundCheckRadius = 0.5f;
    public LayerMask groundLayer;

    public Animator anim;

    public override void Move(Vector2 direction)
    {
        float targetVelocityX = direction.x * speed * Time.deltaTime;
        float smoothVelocityX = Mathf.Lerp(rb.linearVelocity.x, targetVelocityX, 0.1f);
        rb.linearVelocity = new Vector2(smoothVelocityX, rb.linearVelocity.y);

        Jump();

        anim.SetFloat("Magnitude", rb.linearVelocity.magnitude);
        Flip(direction.x);
    }

    public void Jump()
    {
        if (isGrounded() && canJump)
        {
            float yDiff = path.vectorPath[currentWaypoint].y - transform.position.y;
            if (yDiff > jumpThreshold)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                canJump = false;
                StartCoroutine(JumpCooldown());
            }
        }
    }

    public void Flip(float directionX)
    {
        if (directionX > 0.1 && !isFacingRight || directionX < -0.1 && isFacingRight)
        {
            isFacingRight = !isFacingRight;
            Vector3 ls = transform.localScale;
            ls.x *= -1;
            transform.localScale = ls;
        }
    }

    private bool isGrounded()
    {
        return Physics2D.OverlapCircle(groundCheckPosition.position, groundCheckRadius, groundLayer);
    }

    public IEnumerator JumpCooldown()
    {
        yield return new WaitForSeconds(jumpCooldown);
        canJump = true;
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheckPosition.position, groundCheckRadius);
    }
}
