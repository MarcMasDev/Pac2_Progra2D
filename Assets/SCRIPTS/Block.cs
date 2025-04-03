using System;
using System.Collections;
using UnityEngine;

[System.Serializable]
public class Bonus
{
    public GameObject item;
    [Range(0f, 1f)] public float appearanceRate;
}

public enum BlockType
{
    special,
    used,
    endBlock,
    solid
}

public class Block : MonoBehaviour
{
    public BlockType blockType;
    [SerializeField] private Sprite usedBlock;
    [SerializeField] private string tagActivation = "Player";
    [SerializeField] private Transform detectionPoint;
    [SerializeField] private float detectionRadius = 0.2f;

    //Cada block pot tenir el seu bonus, això ens dona control sobre el level design i hi podem afegir randomness si volem.
    [Header("Bonus")]
    [SerializeField] private Transform spawnPos;
    [SerializeField] private Bonus[] bonuses;
    [SerializeField] private int amount = 1;

    //S'encarrega de moure el bloc al colisionar amb el jugador per fer un efecte similar al que fa Mario.
    [Header("Bump")]
    [SerializeField] private float bumpHeight = 0.2f;
    [SerializeField] private float bumpSpeed = 4f;

    //Destroy VFXs
    [Header("Effects")]
    [SerializeField] private GameObject particles;

    private bool givenBonus = false;
    private Vector3 originalPosition;
    private bool isBumping = false;
    private bool destroying = false;

    private void Start()
    {
        originalPosition = transform.localPosition;
    }

    private void Update()
    {
        DetectAndHandleHit();
    }

    private void DetectAndHandleHit()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(detectionPoint.position, detectionRadius);

        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].CompareTag(tagActivation))
            {
                HandleHit(hits[i].gameObject);
            }
        }
    }

    private void HandleHit(GameObject col)
    {
        switch (blockType)
        {
            case BlockType.special:
                HandleSpecialBlock();
                break;

            case BlockType.solid:
                HandleSolidBlock(col);
                break;

            case BlockType.endBlock:
                HandleEndBlock();
                break;
        }
    }

    private void HandleSpecialBlock()
    {
        GiveBonus();
        ChangeBlockToUsed(BlockType.used);
        StartCoroutine(BumpBlock());
    }

    private void HandleSolidBlock(GameObject col)
    {
        if (ShouldDestroy(col) && !destroying) //si el jugador té el mushrrom aplicat destrueix el block.
        {
            destroying = true;
            if (particles != null) Instantiate(particles, transform.position, Quaternion.identity);
            Destroy(gameObject, 0.1f);
        }
        else if (!givenBonus) //els bonus només es donen un cop tot i que el block sigui solid.
        {
            GiveBonus();
        }
        if (!isBumping) StartCoroutine(BumpBlock());
    }
    private void HandleEndBlock()
    {
        if (!givenBonus) GiveBonus();
    }

    private bool ShouldDestroy(GameObject col)
    {
        PowerUps powerUps = col.GetComponent<PowerUps>();
        return powerUps != null && powerUps.mushroomApplied;
    }

    //Used no tenen cap efecte al colisionar, tampoc tenen bump
    private void ChangeBlockToUsed(BlockType blockType)
    {
        this.blockType = blockType;

        if (blockType == BlockType.used && usedBlock != null)
        {
            GetComponent<SpriteRenderer>().sprite = usedBlock;
        }
    }


    private void GiveBonus()
    {
        givenBonus = true;
        GameObject toSpawn = GetRandomBonus();

        if (toSpawn == null) return;

        for (int i = 0; i < amount; i++)
        {
            Instantiate(toSpawn, spawnPos.position, Quaternion.identity);
        }
    }

    //Calcula les probabilitats i dona un bonus aleatori.
    private GameObject GetRandomBonus()
    {
        float totalWeight = CalculateTotalWeight();

        float randomValue = UnityEngine.Random.Range(0f, totalWeight);
        float cumulative = 0f;

        for (int i = 0; i < bonuses.Length; i++)
        {
            cumulative += bonuses[i].appearanceRate;
            if (randomValue <= cumulative)
            {
                return bonuses[i].item;
            }
        }

        return bonuses[0].item;
    }

    private float CalculateTotalWeight()
    {
        float total = 0f;
        for (int i = 0; i < bonuses.Length; i++)
        {
            total += bonuses[i].appearanceRate;
        }
        return total;
    }

    private IEnumerator BumpBlock()
    {
        if (isBumping) yield break;
        isBumping = true;

        Vector3 targetPos = originalPosition + Vector3.up * bumpHeight;

        //Ves cap a dalt
        while (Vector3.Distance(transform.localPosition, targetPos) > 0.01f)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetPos, bumpSpeed * Time.deltaTime);
            yield return null;
        }

        //Ves cap abaix
        while (Vector3.Distance(transform.localPosition, originalPosition) > 0.01f)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, originalPosition, bumpSpeed * Time.deltaTime);
            yield return null;
        }

        transform.localPosition = originalPosition;

        
        //cooldown ajuda a prevenir errors, anteriorment el jugador podia donar-li varios cops i executa multiples corutines que provocaven errors.
        yield return new WaitForSeconds(0.4f);
        isBumping = false;
    }

    private void OnDestroy()
    {
        AudioController.Instance.Play(SoundType.BlockDestroyed);
    }
}
