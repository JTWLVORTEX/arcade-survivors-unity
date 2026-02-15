using UnityEngine;

/**
 * PlayerMovement
 * --------------
 * Top-down movement for a survivors-style game:
 * - WASD / Arrow keys
 * - Normalized diagonal movement (no faster diagonals)
 * - Smooth acceleration/deceleration
 * - Rigidbody2D-based (recommended)
 *
 * Attach to: Player GameObject
 * Requires: Rigidbody2D
 *
 * Inspector:
 * - Set Rigidbody2D Gravity Scale = 0
 * - Optional: Freeze Rotation Z (recommended)
 */
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float maxSpeed = 6f;
    [SerializeField] private float acceleration = 40f;
    [SerializeField] private float deceleration = 50f;

    private Rigidbody2D rb;
    private Vector2 input;
    private Vector2 velocity;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        // Good defaults for top-down movement
        rb.gravityScale = 0f;
        rb.freezeRotation = true;
    }

    private void Update()
    {
        // Raw gives snappy input; we smooth with acceleration ourselves
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        input = new Vector2(x, y);

        // Normalize so diagonals aren't faster
        if (input.sqrMagnitude > 1f)
            input.Normalize();
    }

    private void FixedUpdate()
    {
        // Target velocity based on input
        Vector2 targetVelocity = input * maxSpeed;

        // Choose accel vs decel depending on whether player is trying to move
        float rate = (input.sqrMagnitude > 0.001f) ? acceleration : deceleration;

        // Smoothly move current velocity toward target velocity
        velocity = Vector2.MoveTowards(rb.linearVelocity, targetVelocity, rate * Time.fixedDeltaTime);
        rb.linearVelocity = velocity;
    }
}
