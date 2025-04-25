using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Animator))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 50f;
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float decelerateSpeed = 2f;
    [SerializeField] private float jumpForce = 15f; 
    [SerializeField] private float maxJumpTime = 0.3f;
    private float jumpTimeCounter;
    private bool isJumping;

    [SerializeField] private Transform groundDetector;
    [SerializeField] private float groundRadius;

    public Action OnReachedEndOfLevel;
    public Action OnReachedCheckpoint;

    private Rigidbody2D rb;
    private Animator animator;

    bool isMoving, grounded, facingLeft;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        HealthSystem.OnDamaged += OnAnyHealthDamaged;
    }
    private void OnDisable()
    {
        HealthSystem.OnDamaged -= OnAnyHealthDamaged;
    }

    private void Update()
    {
        isMoving = false;
        grounded = IsGrounded();

        HandleMovement();

        HandleJumping();

        HandleAnimator(!grounded, isMoving);


    }

    //Input moviment
    private void HandleMovement()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            MoveBackward();
            isMoving = true;

            if (!facingLeft)
                FlipPlayer();
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            MoveForward();
            isMoving = true;

            if (facingLeft)
                FlipPlayer();
        }
    }

    //Input i comprobaci� de salt
    private void HandleJumping()
    {
        if (grounded)
        {
            if (!isMoving)
            {
                Decelerate();
            }

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                isJumping = true;
                jumpTimeCounter = maxJumpTime;

                AudioController.Instance.Play(SoundType.Jump);
                Jump();
            }
        }

        HandleHoldJump();
    }
    //Funci� extra per a que el salt es senti millor i m�s similar al de Mario (si mantens, segueix santant).
    private void HandleHoldJump()
    {
        if (Input.GetKey(KeyCode.UpArrow) && isJumping)
        {
            HoldJump();
        }

        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            isJumping = false;
        }
    }
    private void HoldJump()
    {
        if (jumpTimeCounter > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpTimeCounter -= Time.deltaTime;
        }
        else
        {
            isJumping = false;
        }
    }

    private void MoveForward()
    {
        rb.velocity += new Vector2(moveSpeed * Time.deltaTime, 0f);

        if (rb.velocity.x > maxSpeed)
        {
            rb.velocity = new Vector2(maxSpeed, rb.velocity.y);
        }
    }

    private void MoveBackward()
    {
        rb.velocity -= new Vector2(moveSpeed * Time.deltaTime, 0f);

        if (rb.velocity.x < -maxSpeed)
        {
            rb.velocity = new Vector2(-maxSpeed, rb.velocity.y);
        }
    }

    //Per evitar que el jugador patini, es pot fer un physics material o programar manualment la fricci�. La veritat es que el physics material no em va convencer per les seves afectacions adicionals amb la f�sica del motor, aix� que n'he programat un.
    private void Decelerate()
    {
        Vector2 vel = rb.velocity;
        vel.x = Mathf.Lerp(vel.x, 0, decelerateSpeed * Time.deltaTime);
        rb.velocity = vel;
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircleAll(groundDetector.position, groundRadius * 1.1f).Length > 1;
    }

    //Salt utilitzant les f�siques del rigidbody.
    public void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    //Tots els casos de colisi� que interacciona el jugador. S'ha fet aqu� per relaci� amb el moviment i per ser proper a un player manager, per� es pot fer en un altre codi.
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("EndOfLevel"))
            OnReachedEndOfLevel?.Invoke();
        else if (other.gameObject.CompareTag("Checkpoint"))
            OnReachedCheckpoint?.Invoke();
    }

    //Salta si el jugador ha fet mal a un enemic.
    private void OnAnyHealthDamaged(HealthSystem victim, GameObject source)
    {
        if (victim.CompareTag("Enemy") && source == gameObject)
        {
            Jump();
        }
    }

    //Aqu� activem els parametres del animator
    private void HandleAnimator(bool jumping, bool moving)
    {
        animator.SetBool("Jumping", jumping);
        animator.SetBool("Moving", moving);
    }

    //La direcci� del player ha d'afectar als seus childs (d'aquesta forma el shoot el segueix)
    private void FlipPlayer()
    {
        facingLeft = !facingLeft;
        UpdateVisualScale(transform.localScale);
    }


    //Per evitar errors de mushroom i turn a la vegada.
    private void UpdateVisualScale(Vector3 baseSize)
    {
        Vector3 newScale = baseSize;
        if (facingLeft) newScale.x = Mathf.Abs(baseSize.x) * -1;
        else newScale.x = Mathf.Abs(baseSize.x) * 1;

        transform.localScale = newScale;
    }
}
