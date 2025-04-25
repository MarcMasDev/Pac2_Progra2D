using UnityEngine;

public enum EnemyState
{
    Freeze,
    Chase,
    Charge,
    Attack
}

public class FlyingEnemyFSM : MonoBehaviour
{
    [SerializeField] private EnemyState currentState = EnemyState.Freeze;
    [SerializeField] private float chaseDistance = 7f;
    [SerializeField] private float chargeDistance = 3f; // Dist�ncia per comen�ar a carregar
    [SerializeField] private float chargeTime = 1f;     // Duraci� de la c�rrega
    [SerializeField] private float attackSpeed = 5f;    // Velocitat del dash
    [SerializeField] private float chaseSpeed = 2f;    // Velocitat de persecuci�

    private GameObject player;
    private Rigidbody2D rb;
    private float chargeTimer;
    private bool isCharging = false;
    private Vector2 attackTargetPosition;
    private float distanceToPlayer;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        if (rb != null) rb.bodyType = RigidbodyType2D.Kinematic;
    }

    private void Update()
    {
        if (player == null || rb == null) return;

        distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

        switch (currentState)
        {
            case EnemyState.Freeze:
                HandleFreeze();
                break;
            case EnemyState.Chase:
                HandleChase();
                break;
            case EnemyState.Charge:
                HandleCharge();
                break;
            case EnemyState.Attack:
                HandleAttack();
                break;
        }
    }

    private void HandleFreeze()
    {
        rb.velocity = Vector2.zero;
        if (distanceToPlayer < chaseDistance)
        {
            currentState = EnemyState.Chase;
            StartChase();
        }
    }

    private void StartChase()
    {
        if (rb != null) rb.bodyType = RigidbodyType2D.Kinematic;
    }

    private void HandleChase()
    {
        if (distanceToPlayer > chaseDistance)
        {
            currentState = EnemyState.Freeze;
            rb.velocity = Vector2.zero;
            return;
        }

        Vector2 directionToPlayer = (player.transform.position - transform.position).normalized;
        rb.velocity = directionToPlayer * chaseSpeed;

        if (distanceToPlayer < chargeDistance)
        {
            currentState = EnemyState.Charge;
            StartCharge();
        }
    }

    private void StartCharge()
    {
        if (rb != null) rb.bodyType = RigidbodyType2D.Dynamic; //Colisions activades
        isCharging = true;
        chargeTimer = 0f;
        rb.velocity = Vector2.zero; // Stop while charging
        attackTargetPosition = player.transform.position; // Guarda la posici� del jugador per l'atac
    }

    private void HandleCharge()
    {
        if (!isCharging) return;

        chargeTimer += Time.deltaTime;

        // Si el jugador s'allunya durant la c�rrega, torna a Chase
        if (distanceToPlayer > chargeDistance * 1.2f) // Multiplicador per donar una mica de marge
        {
            currentState = EnemyState.Chase;
            isCharging = false;
            StartChase();
            return;
        }

        // Quan el temps de c�rrega s'ha acabat, passa a l'estat d'atac
        if (chargeTimer >= chargeTime)
        {
            currentState = EnemyState.Attack;
            isCharging = false;
            StartAttack();
        }
    }

    private void StartAttack()
    {
        Vector2 dashDirection = (attackTargetPosition - (Vector2)transform.position).normalized;
        rb.velocity = dashDirection * attackSpeed;
    }

    private void HandleAttack()
    {
        // Despr�s de l'atac, torna a l'estat de Freeze.
        if (rb.velocity.magnitude < 0.1f || distanceToPlayer > chargeDistance)
        {
            currentState = EnemyState.Freeze;
        }
    }

    // Funci� per dibuixar gizmos a l'editor per veure les dist�ncies
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseDistance);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chargeDistance);
    }
}