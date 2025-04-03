using UnityEngine;

public class DeadZone : MonoBehaviour
{
    public GameplayManager GameplayManager;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bike"))
        {
            GameplayManager.RestartLevel();
        }
    }
}
