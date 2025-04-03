using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoCollect : MonoBehaviour
{
    [SerializeField] private float timeToCollect = 0.5f;
    [SerializeField] private int amount = 1;
    [SerializeField] private Currencies currency;
    void Start()
    {
        StartCoroutine(Collect());
    }

    //Afageix el "coin" automaticament al comptador del jugador i guarden el valor a la clase inventory.
    private IEnumerator Collect()
    {
        yield return new WaitForSeconds(timeToCollect);

        switch (currency)
        {
            case Currencies.coins:
                Inventory.AddCoins(amount);
                break;
            case Currencies.points:
                Inventory.AddPoints(amount);
                break;
            default:
                break;
        }
        Destroy(gameObject);
    }

}
