using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public PlayerStats playerStats;

    Rigidbody2D rb; Animator anim; SpriteRenderer sr;
    Vector2 move; 
    bool onGround;

    private float speed = 7f;
    private float jumpForce = 12f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        speed = playerStats.speed;
        jumpForce = playerStats.jumpForce;
    }

    // PlayerInput (Behavior: Send Messages) сам вызывает эти методы:
    public void OnMove(InputValue value) { move = value.Get<Vector2>(); }

    public void OnJump()
    {
        if (onGround) rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        onGround = false;
    }

    public void OnFire() { GetComponent<Shooter>().Shoot(); }

    void Update()
    {
        rb.linearVelocity = new Vector2(move.x * speed, rb.linearVelocity.y);
        anim.SetFloat("Speed", Mathf.Abs(move.x));
        if (move.x != 0) sr.flipX = move.x < 0;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground")){
            onGround = true;
            anim.SetBool("IsJump", false);
        }
        if (other.gameObject.CompareTag("Enemy"))
            gameObject.GetComponent<Health>().TakeDamage(1);
    }
}