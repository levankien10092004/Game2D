using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    [SerializeField] private static GameTimer Instance;  

    [SerializeField] private TextMeshProUGUI timerText;

    private float elapsedTime = 0f;
    private bool isRunning = false;

    private void Awake()
    {

        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        ResetTimer();
        StartTimer();
    }

    private void Update()
    {
        if (!isRunning) return;

        elapsedTime += Time.deltaTime;
        UpdateUI();
    }

    public void StartTimer()
    {
        isRunning = true;
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    public void ResetTimer()
    {
        elapsedTime = 0f;
        UpdateUI();
    }

    public float GetTime()
    {
        return elapsedTime;
    }


    private void UpdateUI()
    {
        if (timerText == null) return;

        int minutes = (int)(elapsedTime / 60);
        int seconds = (int)(elapsedTime % 60);

        timerText.text = minutes.ToString("00") + ":" + seconds.ToString("00");
    }
}
