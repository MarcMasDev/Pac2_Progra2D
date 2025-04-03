using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    [SerializeField] private HandleEnd end;
    private void OnEnable()
    {
        HealthSystem.OnKilled += HandleDeath;
        if (end != null) end.OnEnd += LoadWin;
    }

    private void OnDisable()
    {
        HealthSystem.OnKilled -= HandleDeath;
        if (end != null) end.OnEnd -= LoadWin;
    }
    private void HandleDeath(HealthSystem source)
    {
        if (source.CompareTag("Player"))
        {
            LoadGameOver();
        }
    }
    
    public void LoadGame()
    {
        Inventory.ResetCoins();
        Inventory.ResetPoints();
        SceneManager.LoadScene("Game");
    }
    private void LoadGameOver()
    {
        AudioController.Instance.Play(SoundType.GameOver);
        SceneManager.LoadScene("GameOver");
    }
    private void LoadWin()
    {
        AudioController.Instance.Play(SoundType.PowerUp);
        SceneManager.LoadScene("Win");
    }
}
