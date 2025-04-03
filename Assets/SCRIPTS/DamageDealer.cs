using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    [SerializeField] private int damage = 1;
    [SerializeField] private string[] toDamageTag;
    [SerializeField] private Transform damagePoint = null;
    public Vector2 damageSize;

    private void Awake()
    {
        if (damagePoint == null) damagePoint = transform;
    }

    private void Update()
    {
        DealDamage();
    }

    //Aplica damage a tots els objectes propers
    private void DealDamage()
    {
        Collider2D[] colliders = GetEntitiesToDamage();

        for (int i = 0; i < colliders.Length; i++)
        {
            if (DamageToTag(colliders[i].transform))
            {
                IDamageable damageable = colliders[i].GetComponent<IDamageable>();
                if (damageable != null)
                {
                    damageable.TakeDamage(damage, gameObject);
                }
            }
        }
    }

    //Comproba els objectes propers sense dependre de colliders.
    private Collider2D[] GetEntitiesToDamage()
    {
        Vector2 size = damageSize;
        return Physics2D.OverlapBoxAll(damagePoint.position, size, 0f);
    }


    //Permet que un objecte pugui fer damage a multiples objectes
    private bool DamageToTag(Transform t)
    {
        for (int i = 0; i < toDamageTag.Length; i++)
        {
            if (t.CompareTag(toDamageTag[i])) return true;
        }
        return false;
    }

    //Permet veure al editor els damage radius.
    private void OnDrawGizmosSelected()
    {
        if (damagePoint == null) damagePoint = transform;

        Gizmos.color = Color.red;
        Vector3 size = damageSize;
        Gizmos.DrawWireCube(damagePoint.position, size);
    }
}
