using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class GenericMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private float speed = 2;

    [SerializeField] private Transform rightDetector, leftDetector;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float detectionRadius = 0.1f;

    [SerializeField] private bool dirRight = true;
    [SerializeField] private bool moveIfPlayerNear = true;
    [SerializeField] private float activationDistance = 8f;
    private GameObject player;

    [SerializeField] private float flipCooldown = 0.2f;
    [SerializeField] private float flipTimer = 0f;
    [SerializeField] private float dist = 1000f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player");

        if (!dirRight) Flip();
    }

    private void Update()
    {
        flipTimer -= Time.deltaTime; //cooldown per evitar que el punt de col·lisió detecti un obstacle anterior.

        //El mario original té punts on sembla que els enemics només es mouen quan mario és a prop, per això he inclos això. Aturar el moviment d'objectes si el player es lluny.
        if (moveIfPlayerNear && !IsPlayerNearby())
        {
            rb.velocity = new Vector2(0f, rb.velocity.y);
            return;
        }

        //moviment rectilini uniforme en el eix X
        rb.velocity = new Vector2(speed, rb.velocity.y);

        CheckCollisions();
    }

    //gira el objecte i el canvia de direcció al tocar una pared
    private void CheckCollisions()
    {
        if (flipTimer > 0f) return;

        bool touchingWall = IsColliding();

        if (touchingWall)
        {
            Flip();
            flipTimer = flipCooldown;
        }
    }

    private void Flip()
    {
        speed *= -1;
        spriteRenderer.flipX = !spriteRenderer.flipX;
        dirRight = !dirRight;
    }

    private bool IsColliding() 
    {
        return Physics2D.OverlapCircle(GetDetectorPosition(), detectionRadius, groundLayer);
    }
    private Vector3 GetDetectorPosition()
    {
        if (dirRight) return leftDetector.position; ;
        return rightDetector.position;
    }
    private bool IsPlayerNearby()
    {
        if (player) dist = Vector2.Distance(player.transform.position, transform.position);
        return dist <= activationDistance;
    }
}
