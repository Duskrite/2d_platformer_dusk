using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float playerSpeed;
    public float playerSpeedMultiplier = 1f;
    public float horizontalMovement;
    public bool isFacingRight = true;

    [Header("Jumping")]
    public float jumpForce;
    public int maxJumps = 2;
    public int jumpRemainings;

    [Header("Dash")]
    public float dashSpeed = 20f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 2f;
    public bool isDashing;
    public bool canDash;
    public TrailRenderer trailRenderer;

    [Header("Wall Movement")]
    public float wallSlideSpeed;
    public bool isWallSliding;

    // Wall jumping
    public bool isWallJumping;
    public float wallJumpDirection;
    public float wallJumpTime = 0.2f;
    public float wallJumpTimer;
    public Vector2 wallJumpForce;

    [Header("Gravity")]
    public float baseGravity = 2f;
    public float maxFallSpeed = 20f;
    public float fallSpeedMultiplier = 2f;

    [Header("Dropdown")]
    public bool isOnPlatfrom;
    public float disableTime = 0.5f;

    [Header("Ground Check")]
    public bool isGrounded;
    public Transform groundCheck;
    public Vector2 groundCheckSize;
    public LayerMask groundLayer;

    [Header("Wall Check")]
    public Transform wallCheck;
    public Vector2 wallCheckSize;
    public LayerMask wallLayer;

    [Header("Reference")]
    public Rigidbody2D rb;
    public Animator anim;
    public BoxCollider2D playerCollider;
    public InGameUIController inGameUIController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canDash = true;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        trailRenderer = GetComponent<TrailRenderer>();
        playerCollider = GetComponent<BoxCollider2D>();

        SpeedBoostItem.OnSpeedBoostItemCollected += MultiplySpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDashing)
        {
            ProcessGroundCheck();
            ProcessGravity();
            ProcessWallSlide();
            ProcessWallJump();

            anim.SetFloat("magnitude", rb.linearVelocity.magnitude);
            anim.SetFloat("yVelocity", rb.linearVelocity.y);
            anim.SetBool("isWallSliding", isWallSliding);

            if (!isWallJumping)
            {
                MovePlayer();
                FlipPlayer();
            }
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        horizontalMovement = context.ReadValue<Vector2>().x;
    }

    public void Dash(InputAction.CallbackContext context)
    {
        if (context.performed && canDash)
        {
            StartCoroutine(DashCoroutine());
        }
    }

    public void Dropdown(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded && isOnPlatfrom)
        {
            StartCoroutine(DropdownCoroutine());
        }
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (jumpRemainings > 0)
        {
            if (context.performed)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                anim.SetTrigger("jump");
            }
            else if (context.canceled)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
                jumpRemainings--;
                anim.SetTrigger("jump");
            }
        }

        // Wall jumping
        if (context.performed && wallJumpTimer > 0 && isWallSliding)
        {
            isWallJumping = true;
            rb.linearVelocity = new Vector2(wallJumpDirection * wallJumpForce.x, wallJumpForce.y);
            wallJumpTimer = 0;
            anim.SetTrigger("jump");

            if (transform.localScale.x != wallJumpDirection)
            {
                isFacingRight = !isFacingRight;
                Vector2 ls = transform.localScale;
                ls.x *= -1;
                transform.localScale = ls;
            }

            Invoke(nameof(CancelWallJump), wallJumpTime + 0.1f);
        }
    }

    public void MovePlayer()
    {
        rb.linearVelocity = new Vector2(horizontalMovement * playerSpeed * playerSpeedMultiplier, rb.linearVelocity.y);
    }

    public void FlipPlayer()
    {
        if (horizontalMovement < 0 && isFacingRight || horizontalMovement > 0 && !isFacingRight)
        {
            isFacingRight = !isFacingRight;
            Vector2 ls = transform.localScale;
            ls.x *= -1;
            transform.localScale = ls;
        }
    }

    public void ProcessWallJump()
    {
        if (isWallSliding)
        {
            wallJumpDirection = -transform.localScale.x;
            wallJumpTimer = wallJumpTime;
            CancelInvoke(nameof(CancelWallJump));
        }
        else if (wallJumpTimer > 0)
        {
            wallJumpTimer -= Time.deltaTime;
        }
        else if (isGrounded)
        {
            wallJumpTimer = 0;
            isWallJumping = false;
        }
    }

    public IEnumerator DashCoroutine()
    {
        Physics2D.IgnoreLayerCollision(7, 8, true);
        isDashing = true;
        canDash = false;
        rb.linearVelocity = new Vector2((isFacingRight ? 1f : -1f) * dashSpeed, rb.linearVelocityY);

        trailRenderer.emitting = true;
        //anim.SetTrigger("dash");
        yield return new WaitForSeconds(dashDuration);
        Physics2D.IgnoreLayerCollision(7, 8, false);

        isDashing = false;
        trailRenderer.emitting = false;
        rb.linearVelocity = new Vector2(0, rb.linearVelocityY);

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    public IEnumerator DropdownCoroutine()
    {
        playerCollider.enabled = false;
        yield return new WaitForSeconds(disableTime);
        playerCollider.enabled = true;
        isOnPlatfrom = false;
    }

    public void MultiplySpeed(float speedMultiplier, float duration)
    {
        StartCoroutine(MuliplySpeedCoroutine(speedMultiplier, duration));
    }

    public IEnumerator MuliplySpeedCoroutine(float speedMultiplier, float duration)
    {
        playerSpeedMultiplier *= speedMultiplier;

        trailRenderer.emitting = true;
        yield return new WaitForSeconds(duration);
        playerSpeedMultiplier = 1f;
        trailRenderer.emitting = false;
    }


    public void CancelWallJump()
    {
        isWallJumping = false;
    }

    public void ProcessGroundCheck()
    {
        if (Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0, groundLayer))
        {
            isGrounded = true;
            jumpRemainings = maxJumps;
        }
        else
        {
            isGrounded = false;
        }
    }

    public bool ProcessWallCheck()
    {
        return Physics2D.OverlapBox(wallCheck.position, wallCheckSize, 0, wallLayer);
    }

    public void ProcessGravity()
    {
        if (rb.linearVelocity.y < 0)
        {
            rb.gravityScale = baseGravity * fallSpeedMultiplier;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Max(rb.linearVelocity.y, -maxFallSpeed));
        }
        else
        {
            rb.gravityScale = baseGravity;
        }
    }

    public void ProcessWallSlide()
    {
        if (!isGrounded && ProcessWallCheck() && horizontalMovement != 0)
        {
            isWallSliding = true;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Max(rb.linearVelocity.y, -wallSlideSpeed));
        }
        else
        {
            isWallSliding = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            isOnPlatfrom = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Door"))
        {
            isOnPlatfrom = true;
            inGameUIController.Victory();
        }
    }


    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(groundCheck.position, groundCheckSize);

        Gizmos.DrawCube(wallCheck.position, wallCheckSize);
    }
}
