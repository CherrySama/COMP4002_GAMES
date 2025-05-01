using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class TimerController : MonoBehaviour
{
    [Header("Setting")]
    [SerializeField] public bool isCountdown = true;         // 是否为倒计时模式
    [SerializeField] public float gameDuration = 300f;       // 游戏持续时间（秒），倒计时模式下为初始时间
    [SerializeField] private bool isPaused = false;           // 计时器是否暂停

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI timerText;       // 显示时间的文本组件
    [SerializeField] private Image timerFillImage;            // 可选：时间进度条

    [Header("Event Setting")]
    [SerializeField] private float[] difficultyIncreaseTimePoints = { 60f, 120f, 180f, 240f }; // 难度提升的时间点（秒）

    // 私有变量
    private float currentTime;                                // 当前剩余/经过的时间
    private int currentDifficultyLevel = 0;                   // 当前难度等级

    // 事件 - 可以在其他脚本中订阅这些事件
    public event Action OnTimerEnd;                           // 计时结束时触发
    public event Action<int> OnDifficultyIncrease;            // 难度提升时触发，参数为新的难度等级

    // 用于获取当前时间，其他脚本可以访问
    public float CurrentTime => currentTime;
    public int CurrentDifficultyLevel => currentDifficultyLevel;

    private void Start()
    {
        InitializeTimer();
    }

    private void Update()
    {
        if (isPaused) return;

        UpdateTimer();
        UpdateTimerDisplay();
        CheckDifficultyIncrease();
    }

    private void InitializeTimer()
    {
        currentTime = isCountdown ? gameDuration : 0f;
        currentDifficultyLevel = 0;
        UpdateTimerDisplay();
    }

    private void UpdateTimer()
    {
        if (isCountdown)
        {
            currentTime -= Time.deltaTime;

            if (currentTime <= 0f)
            {
                currentTime = 0f;
                isPaused = true;
                OnTimerEnd?.Invoke();
            }
        }
        else
        {
            currentTime += Time.deltaTime;

            if (currentTime >= gameDuration)
            {
                currentTime = gameDuration;
                isPaused = true;
                OnTimerEnd?.Invoke();
            }
        }
    }

    private void UpdateTimerDisplay()
    {
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(currentTime / 60f);
            int seconds = Mathf.FloorToInt(currentTime % 60f);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }

        if (timerFillImage != null)
        {
            float fillAmount = isCountdown ?
                currentTime / gameDuration :
                1f - (currentTime / gameDuration);

            timerFillImage.fillAmount = fillAmount;
        }
    }

    private void CheckDifficultyIncrease()
    {
        if (currentDifficultyLevel >= difficultyIncreaseTimePoints.Length) return;

        float timeToCheck = isCountdown ?
            gameDuration - currentTime :  // 倒计时模式：已经过去的时间
            currentTime;                  // 正计时模式：已经过去的时间

        if (timeToCheck >= difficultyIncreaseTimePoints[currentDifficultyLevel])
        {
            currentDifficultyLevel++;
            OnDifficultyIncrease?.Invoke(currentDifficultyLevel);

        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
    }

    public void ResetTimer()
    {
        InitializeTimer();
        isPaused = false;
    }

    public void SetCountdownMode(bool isCountdownMode)
    {
        if (this.isCountdown != isCountdownMode)
        {
            this.isCountdown = isCountdownMode;
            InitializeTimer();
        }
    }

    public void SetGameDuration(float duration)
    {
        gameDuration = duration;
        InitializeTimer();
    }
}