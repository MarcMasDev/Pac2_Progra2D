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
    }
    private void EnableMushroomEffect()
    {
        transform.localScale = new Vector2(sizeIncrease.x + transform.localScale.x, sizeIncrease.y + transform.localScale.y);
        healthSystem.Heal(healAmount);

        AudioController.Instance.Play(SoundType.PowerUp);
    }
    private void DisableMushroomEffect()
    {
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
