using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 8f;
    public float jumpForce = 18f;
    private float moveInput;
    private bool isFacingRight = true;

    [Header("Dash")]
    public float dashingPower = 15f;
    public float dashingTime = 0.125f;
    public float dashingCooldown = 1f;
    private bool canDash = true;
    private bool isDashing;
    public TrailRenderer trail;

    [Header("Wall Slide & Jump")]
    public float wallSlidingSpeed = 2f;
    public float wallJumpingTime = 0.3f;      
    public float wallJumpingDuration = 0.4f;  
    public Vector2 wallJumpingPower = new Vector2(12f, 20f);
    private bool isWallSliding;
    private bool isWallJumping;
    private float wallJumpingCounter;
    private float wallJumpingDirection;

    [Header("Checks & Layers")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    public float groundCheckRadius = 0.5f;
    public float wallCheckRadius = 0.5f;
    [SerializeField] private TrailRenderer tr;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        trail.startWidth = 10f;
        trail.endWidth = 8f;
    }

    void Update()
    {
        if (isDashing) return;

        moveInput = Input.GetAxis("Horizontal");

        HandleJump();
        HandleWallSlide();
        HandleWallJump();

        if (!isWallJumping)
            Flip();

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
            StartCoroutine(Dash());

        
        if (!isWallJumping)
            rb.linearVelocity = new Vector2(moveInput * speed, rb.linearVelocity.y);
    }

    // --- Jump ---


    private void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        // Variable jump height
        if (Input.GetButtonUp("Jump") && rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
        }
    }

    // --- Ground Check ---
    private bool IsGrounded()
    {
        bool grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        Debug.Log("IsGrounded: " + grounded);
        return grounded;
    }

    // --- Wall Check ---
    private bool IsWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, wallCheckRadius, wallLayer);
    }

    // --- Wall Slide ---
    private void HandleWallSlide()
    {
        if (IsWalled() && moveInput != 0f && !IsGrounded())
        {
            isWallSliding = true;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Clamp(rb.linearVelocity.y, -wallSlidingSpeed, float.MaxValue));
        }
        else
        {
            isWallSliding = false;
        }
    }

    // --- Wall Jump ---
    private void HandleWallJump()
    {
        // Reset counter if sliding
        if (isWallSliding)
        {
            isWallJumping = false;
            wallJumpingCounter = wallJumpingTime;
            wallJumpingDirection = isFacingRight ? -1f : 1f; // Use isFacingRight, not transform scale
            CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }

        // Perform wall jump
        if (Input.GetButtonDown("Jump") && wallJumpingCounter > 0f)
        {
            isWallJumping = true;
            rb.linearVelocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f;

            // Flip player if necessary
            if ((wallJumpingDirection > 0 && !isFacingRight) || (wallJumpingDirection < 0 && isFacingRight))
                Flip();

            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }
    }

    private void StopWallJumping()
    {
        isWallJumping = false;
    }

    // --- Flip Character ---
    private void Flip()
    {
        if ((isFacingRight && moveInput < 0f) || (!isFacingRight && moveInput > 0f))
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    // --- Dash ---
    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;

        rb.linearVelocity = new Vector2((isFacingRight ? 1 : -1) * dashingPower, 0f);
        tr.emitting = true;

        yield return new WaitForSeconds(dashingTime);

        tr.emitting = false;
        rb.gravityScale = originalGravity;
        isDashing = false;

        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }

    // --- Debug Visualization ---
    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        if (wallCheck != null)
            Gizmos.DrawWireSphere(wallCheck.position, wallCheckRadius);
    }

}