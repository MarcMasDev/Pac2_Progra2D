using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    [SerializeField] private SceneController sceneController;
    [SerializeField] private float startingTime = 999f;
    private float currentTime;
    private bool isRunning = true;

    [SerializeField] private Text timeText;

    private void Start()
    {
        currentTime = startingTime;
        UpdateUI();
    }

    private void Update()
    {
        if (!isRunning) return;

        currentTime -= Time.deltaTime;

        if (currentTime <= 0f)
        {
            currentTime = 0f;
            isRunning = false;

            if (sceneController != null)
            {
                sceneController.SendMessage("LoadGameOver");
            }
        }

        UpdateUI();
    }

    private void UpdateUI()
    {
        int seconds = Mathf.CeilToInt(currentTime);
        timeText.text = $"Time: {seconds}";
    }

}
