using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimerController : MonoBehaviour
{
    [SerializeField] public float startTime = 300.0f; // 设置开始时间为300秒（5分钟）
    private float currentTime;
    private bool isTimerRunning = true;

    [SerializeField] private TextMeshProUGUI timerText;

    void Start()
    {
        currentTime = startTime;
        UpdateTimerDisplay();
    }

    void Update()
    {
        if (isTimerRunning)
        {
            currentTime -= Time.deltaTime;

            if (currentTime <= 0)
            {
                currentTime = 0;
                isTimerRunning = false;
                OnTimeUp();
            }

            UpdateTimerDisplay();
        }
    }

    private void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private void OnTimeUp()
    {
        Debug.Log("Time's up! Game over.");
        Messenger.Broadcast(GameEvent.PLAYER_DEAD); // 广播游戏结束事件
    }
}
