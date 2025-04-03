using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameplayManager : MonoBehaviour
{
    public BikeController BikeController;
    public Text TimeText;
    public Text RecordText;
    public Button StartButton;

    private float initialTime;
    private float recordTime;
    private int secondsToStart = 3;

    private void Start()
    {
        TimeText.text = RecordText.text = string.Empty;

        BikeController.OnKilled += RestartLevel;
        BikeController.OnReachedEndOfLevel += EndGame;
        BikeController.enabled = false;

        recordTime = PlayerPrefs.GetFloat("recordLevel" + SceneManager.GetActiveScene().buildIndex, 0);

        if (recordTime > 0)
            RecordText.text = "Record: " + recordTime.ToString("00.00") + "s";
    }

    private void Update()
    {
        if (BikeController.enabled)
        {
            TimeText.text = "Time: " + (Time.time - initialTime).ToString("00.00") + "s";
        }
    }

    public void StartGame()
    {
        StartButton.gameObject.SetActive(false);

        TimeText.text = secondsToStart.ToString();
        InvokeRepeating(nameof(Countdown), 1, 1);
    }

    private void Countdown()
    {
        secondsToStart--;
        if (secondsToStart <= 0)
        {
            CancelInvoke();
            OnGameStarted();
        }
        else
            TimeText.text = secondsToStart.ToString();
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnGameStarted()
    {
        BikeController.enabled = true;
        initialTime = Time.time;
        TimeText.text = string.Empty;
    }

    private void EndGame()
    {
        StartButton.gameObject.SetActive(true);

        BikeController.enabled = false;
        TimeText.text = "FINAL! " + (Time.time - initialTime).ToString("00.00") + "s";

        if ((Time.time - initialTime) < recordTime)
        {
            PlayerPrefs.SetFloat("recordLevel" + SceneManager.GetActiveScene().buildIndex, (Time.time - initialTime));
            TimeText.text = "NEW RECORD! " + (Time.time - initialTime).ToString("00.00") + "s";
        }
        else
        {
            TimeText.text = "FINAL! " + (Time.time - initialTime).ToString("00.00") + "s";
        }
    }
}
