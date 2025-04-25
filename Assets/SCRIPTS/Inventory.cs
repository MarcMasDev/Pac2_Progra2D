//permet guardar dades entre escenes. 
using UnityEngine;

public static class Inventory
{
    public static int Coins { get; private set; } = 0;
    public static int Points { get; private set; } = 0;


    public static bool CheckpointSet { get; private set; } = false;

    public static Vector2 LastCheckpoint { get; private set; }

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

    public static void AddCheckpoint(Vector2 position)
    {
        LastCheckpoint = position;
        CheckpointSet = true;
    }

    public static void ResetCheckpoint()
    {
        CheckpointSet = false;
    }
}
