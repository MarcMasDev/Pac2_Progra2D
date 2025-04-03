using System;
using System.Collections;
using UnityEngine;

//IDamagable fa que tots els objectes amb vida puguin rebre mal.
public class HealthSystem : MonoBehaviour, IDamageable
{
    public static Action<HealthSystem> OnKilled;
    public static Action<HealthSystem, GameObject> OnDamaged; //cada cop que fem mal a un enemic saltem.

    [SerializeField]private int health = 1;

    [Header("Damage VFX")]
    [SerializeField] private float recoveryDuration = 1.0f;
    [SerializeField] private float flickerSpeed = 0.1f;
    [SerializeField] private GameObject particleVFXs;
    private bool isRecovering = false;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public void TakeDamage(int amount, GameObject source)
    {
        if (isRecovering && !source.CompareTag("Instakill")) return; //si el jugador es cau el volem matar si o si.


        health -= amount;
        AudioController.Instance.Play(SoundType.Hit);

        OnDamaged?.Invoke(this, source);

        //VFXs sang
        if (particleVFXs) Instantiate(particleVFXs, transform.position, Quaternion.identity);

        if (health <= 0) //si té 0 o menys (si s'aplica més damage) vida mata.
        {
            OnKilled?.Invoke(this);
            Destroy(gameObject);
        }
        else
        {
            if(transform.CompareTag("Player")) StartCoroutine(Recover()); //si es el player fes recovery, es by far la forma més sencilla de fer-ho tot i que ho podría fer per actions.
        }
    }
    public void Heal(int amount)
    {
        //els comentari en angles els poso mentre programo :) Els deixaré així perquè s'entenen millor.
        health += amount; //we could use takeDamage -1 but then we would be calling OnDamage and I do not think this is as it should. There are workarounds but doing a new void seems the best way (in terms of code clarity)
    }

    //Quan s'aplica damage i no es mata hi ha uns instants de recover. Això s'aplica sobre tots els objectes, si es volgues només pel player 
    //Hauriem de fer una comprobació de si es el player amb un IF(transform.comparetag("player")) típic (pseudocodi)
    private IEnumerator Recover()
    {
        isRecovering = true;
        float elapsed = 0f;

        while (elapsed < recoveryDuration)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(flickerSpeed);
            elapsed += flickerSpeed;
        }

        spriteRenderer.enabled = true;
        isRecovering = false;
    }
}
