using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HealthSystem))]
[RequireComponent(typeof(DamageDealer))]
public class PowerUps : MonoBehaviour
{
    [Header("Mushroom")]
    [SerializeField] private string mushroomTag;
    [SerializeField] private Vector2 sizeIncrease;
    private Vector2 initSize;
    [SerializeField] private int healAmount = 1;
    private HealthSystem healthSystem;
    [HideInInspector] public bool mushroomApplied = false;

    [Header("Coins")]
    [SerializeField] private string coinsTag;
    [SerializeField] private int coinsAmount = 1;

    [Header("Checkpoints")]
    [SerializeField] private string checkpointsTag;
    private void OnEnable()
    {
        HealthSystem.OnDamaged += LostHealth;
    }

    private void OnDisable()
    {
        HealthSystem.OnDamaged -= LostHealth;
    }
    private void Awake()
    {
        initSize = transform.localScale;
        healthSystem = GetComponent<HealthSystem>();

        if (Inventory.CheckpointSet) transform.position = Inventory.LastCheckpoint;
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag(mushroomTag))
        {
            if (!mushroomApplied)
            {
                mushroomApplied = true;
                Destroy(col.gameObject);
                EnableMushroomEffect();
            } else Destroy(col.gameObject);
        }
        if (col.CompareTag(coinsTag))
        {
            Inventory.AddCoins(coinsAmount);
            Destroy(col.gameObject);
        }
        if (col.CompareTag(checkpointsTag))
        {
            Inventory.AddCheckpoint(col.transform.position);
            Destroy(col.gameObject);
        }
    }
    private void EnableMushroomEffect()
    {
        float facing = Mathf.Sign(transform.localScale.x); // +1 o -1 segons si està girat

        Vector2 absScale = new Vector2(Mathf.Abs(transform.localScale.x), Mathf.Abs(transform.localScale.y));
        Vector2 newScale = new Vector2(absScale.x + sizeIncrease.x, absScale.y + sizeIncrease.y);
        transform.localScale = new Vector3(newScale.x * facing, newScale.y, 1f);

        healthSystem.Heal(healAmount);
        AudioController.Instance.Play(SoundType.PowerUp);
    }
    private void DisableMushroomEffect()
    {
        float facing = Mathf.Sign(transform.localScale.x); // Recorda direcció actual

        transform.localScale = new Vector3(Mathf.Abs(initSize.x) * facing,Mathf.Abs(initSize.y),1f);
        transform.localScale = initSize;

        AudioController.Instance.Play(SoundType.PowerUpLost);
    }
    private void LostHealth(HealthSystem damageReciever, GameObject source)
    {
        if (damageReciever.CompareTag("Player") && mushroomApplied)
        {
            mushroomApplied = false;
            DisableMushroomEffect();
        }
    }
}
