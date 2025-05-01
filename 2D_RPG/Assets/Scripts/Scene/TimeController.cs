using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class TimerController : MonoBehaviour
{
    [Header("Setting")]
    [SerializeField] public bool isCountdown = true;         // �Ƿ�Ϊ����ʱģʽ
    [SerializeField] public float gameDuration = 300f;       // ��Ϸ����ʱ�䣨�룩������ʱģʽ��Ϊ��ʼʱ��
    [SerializeField] private bool isPaused = false;           // ��ʱ���Ƿ���ͣ

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI timerText;       // ��ʾʱ����ı����
    [SerializeField] private Image timerFillImage;            // ��ѡ��ʱ�������

    [Header("Event Setting")]
    [SerializeField] private float[] difficultyIncreaseTimePoints = { 60f, 120f, 180f, 240f }; // �Ѷ�������ʱ��㣨�룩

    // ˽�б���
    private float currentTime;                                // ��ǰʣ��/������ʱ��
    private int currentDifficultyLevel = 0;                   // ��ǰ�Ѷȵȼ�

    // �¼� - �����������ű��ж�����Щ�¼�
    public event Action OnTimerEnd;                           // ��ʱ����ʱ����
    public event Action<int> OnDifficultyIncrease;            // �Ѷ�����ʱ����������Ϊ�µ��Ѷȵȼ�

    // ���ڻ�ȡ��ǰʱ�䣬�����ű����Է���
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
            gameDuration - currentTime :  // ����ʱģʽ���Ѿ���ȥ��ʱ��
            currentTime;                  // ����ʱģʽ���Ѿ���ȥ��ʱ��

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