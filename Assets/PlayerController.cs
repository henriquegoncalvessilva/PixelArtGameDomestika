using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] int totalHealth = 3;
    [SerializeField] int healthCurrent;
    [SerializeField] float longIdleTime = 5;
    [SerializeField] float longIdleTimer;
    [SerializeField] float speed;
    [SerializeField] float groundCheckRadius;
    [SerializeField] float jumpForce = 2.5f;
    [SerializeField] bool faceFlip = true;
    [SerializeField] bool isGrounded;
    [SerializeField] bool isAttacking;


    [SerializeField] AudioSource soundJumpFX;
    [SerializeField] SpriteRenderer renderer;
    [SerializeField] Rigidbody2D rigd;
    [SerializeField] Animator anim;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Vector2 movement;




    private void Awake()
    {
        rigd = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        renderer = GetComponent<SpriteRenderer>();


    }

    void Start()
    {
        healthCurrent = totalHealth;
    }


    void Update()
    {

        if (!isAttacking)
        {

            float h = Input.GetAxisRaw("Horizontal");
            movement = new Vector2(h, 0f);

            if (h < 0f && faceFlip)
            {
                Flip();
            }
            else if (h > 0f && !faceFlip)
            {
                Flip();
            }

        }

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);


        if (Input.GetButtonDown("Jump") && isGrounded && !isAttacking)
        {
            PlaySoundJump();
            rigd.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        if (Input.GetButtonDown("Fire1") && isGrounded && !isAttacking)
        {
            movement = Vector2.zero;
            rigd.velocity = Vector2.zero;
            anim.SetTrigger("Attack");
        }
    }

    public void PlaySoundJump()
    {
        soundJumpFX.Play();
    }


    public void AddDamage(int value)
    {
        healthCurrent -= value;

        GetComponent<InstantiatePrefabs>().Instance();

        StartCoroutine("VisualFeedback");

        if (healthCurrent <= 0)
        {
            healthCurrent = 0;
        }
    }

    public void AddHealth(int value)
    {
        healthCurrent += healthCurrent;
        if (healthCurrent > totalHealth)
        {
            healthCurrent = totalHealth;
        }
    }

    IEnumerator VisualFeedback()
    {

        renderer.color = Color.red;


        yield return new WaitForSeconds(1);

        renderer.color = Color.white;
    }


    private void FixedUpdate()
    {

        if (!isAttacking)
        {
            float hVelocity = movement.normalized.x * speed;

            rigd.velocity = new Vector2(hVelocity, rigd.velocity.y);
        }
    }

    private void LateUpdate()
    {
        anim.SetBool("Idle", movement == Vector2.zero);
        anim.SetBool("IsGrounded", isGrounded);
        anim.SetFloat("VerticalVelocity", rigd.velocity.y);

        if (anim.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            
            isAttacking = true;

        }
        else
        {
            isAttacking = false;
        }

        if (anim.GetCurrentAnimatorStateInfo(0).IsTag("Idle"))
        {
            longIdleTimer += Time.deltaTime;

            if (longIdleTimer >= longIdleTime)
            {
                anim.SetTrigger("LongIdle");
            }

        }

        else
        {
            longIdleTimer = 0f;
        }
    }


    void Flip()
    {
        faceFlip = !faceFlip;

        float localScaleX = transform.localScale.x;
        localScaleX *= -1f;
        transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z);
    }
}
