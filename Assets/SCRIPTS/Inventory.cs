//permet guardar dades entre escenes. 
public static class Inventory
{
    public static int Coins { get; private set; } = 0;
    public static int Points { get; private set; } = 0;


    public static void AddCoins(int amount)
    {
        Coins += amount;
        AudioController.Instance.Play(SoundType.Coin);
    }

    public static void ResetCoins()
    {
        Coins = 0;
    }

    public static void AddPoints(int amount)
    {
        Points += amount;
    }

    public static void ResetPoints()
    {
        Points = 0;
    }
}
