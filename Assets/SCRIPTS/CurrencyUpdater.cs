using UnityEngine;
using UnityEngine.UI;

public enum Currencies
{
    coins, 
    points
}

//Actualitza la UI de les currencies.
public class CurrencyUpdater : MonoBehaviour
{
    [SerializeField] private Currencies currency;
    private Text displayerText;
    private void Awake()
    {
        displayerText = GetComponent<Text>();
    }
    private void Update()
    {
        switch (currency)
        {
            case Currencies.coins:
                displayerText.text = Inventory.Coins.ToString();
                break;
            case Currencies.points:
                displayerText.text = Inventory.Points.ToString();
                break;
        }
    }
}
