using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BossController : MonoBehaviour
{
    [Header("Boss Setting")]
    [SerializeField] private GameObject bossPrefab; // Boss预制体
    [SerializeField] private Transform bossSpawnPoint; // Boss生成位置
    [SerializeField] private float bossSpawnTime = 300f; // 默认5分钟(300秒)后生成Boss
    [SerializeField] private float bossWarningTime = 30f; // Boss出现前的警告时间(默认30秒)
    [SerializeField] private string bossHealthComponentName = "Health"; // Boss生命值组件名称
    [SerializeField] private string bossHealthPropertyName = "CurrentHealth"; // Boss生命值属性名称

    [Header("Component")]
    [SerializeField] private TimerController timerController;
    [SerializeField] private MonsterRespawnManager monsterRespawnManager; // 用于Event

    [Header("UI")]
    [SerializeField] private GameObject bossWarningPanel; // Boss警告面板
    [SerializeField] private TextMeshProUGUI warningText; // 警告文本
    //[SerializeField] private GameObject bossHealthBar; // Boss血条UI
    //[SerializeField] private Slider bossHealthSlider; // Boss血条滑块
    //[SerializeField] private TextMeshProUGUI bossNameText; // Boss名称文本

    //[Header("Audio")]
    //[SerializeField] private AudioClip warningSound; // 警告音效
    //[SerializeField] private AudioClip bossSpawnSound; // Boss出现音效
    //[SerializeField] private AudioClip bossDefeatedSound; // Boss被击败音效

    [Header("Winning")]
    [SerializeField] private string bossName = "EvilWizard"; // Boss名称
    [SerializeField] private float delayBeforeVictory = 2f; // 击败Boss后显示胜利画面的延迟

    // 私有变量
    private bool bossSpawned = false;
    private bool warningShown = false;
    private bool bossDefeated = false;
    private AudioSource audioSource;
    private GameObject currentBoss;
    private int bossMaxHealth = 100;
    private int bossCurrentHealth = 100;
    private float deadzoneY = -25f;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void Start()
    {
        if (timerController == null)
            timerController = FindFirstObjectByType<TimerController>();

        if (bossWarningPanel != null)
            bossWarningPanel.SetActive(false);

        //if (bossHealthBar != null)
        //    bossHealthBar.SetActive(false);

        //if (bossNameText != null)
        //    bossNameText.text = bossName;
    }

    private void Update()
    {
        if (bossSpawned && !bossDefeated && currentBoss != null)
        {
            if (currentBoss.transform.position.y < deadzoneY)
            {
                WinningPageShown.Instance.ShowGameOver();
                //OnBossDefeated();
                return;
            }

            MonitorBossHealth();
        }

        if (bossSpawned) return;

        float gameTime = GetGameTime();

        if (!warningShown && gameTime >= (bossSpawnTime - bossWarningTime))
        {
            ShowBossWarning();
            warningShown = true;
        }

        if (!bossSpawned && gameTime >= bossSpawnTime)
        {
            SpawnBoss();
            bossSpawned = true;
        }


    }

    private float GetGameTime()
    {
        if (timerController == null) return 0f;

        return timerController.isCountdown
            ? timerController.gameDuration - timerController.CurrentTime
            : timerController.CurrentTime;
    }

    private void ShowBossWarning()
    {
        if (bossWarningPanel != null)
        {
            bossWarningPanel.SetActive(true);

            if (warningText != null)
            {
                warningText.text = $"{bossName} is coming.";
            }

            StartCoroutine(AnimateWarning());
        }

        //if (audioSource != null && warningSound != null)
        //{
        //    audioSource.PlayOneShot(warningSound);
        //}
    }

    private void SpawnBoss()
    {
        if (bossPrefab == null || bossSpawnPoint == null)
        {
            Debug.LogError("Boss is not set or spawnpoint is not set");
            return;
        }

        currentBoss = Instantiate(bossPrefab, bossSpawnPoint.position, bossSpawnPoint.rotation);
        currentBoss.name = bossName;

        EnableAllColliders(currentBoss);

        GetBossInitialHealth();

        if (bossWarningPanel != null)
        {
            bossWarningPanel.SetActive(false);
        }

        //if (bossHealthBar != null)
        //{
        //    bossHealthBar.SetActive(true);

        //    if (bossHealthSlider != null)
        //    {
        //        bossHealthSlider.maxValue = bossMaxHealth;
        //        bossHealthSlider.value = bossCurrentHealth;
        //    }
        //}

        //if (audioSource != null && bossSpawnSound != null)
        //{
        //    audioSource.PlayOneShot(bossSpawnSound);
        //}
    }

    private void GetBossInitialHealth()
    {
        if (currentBoss == null) return;

        Component healthComponent = currentBoss.GetComponent(bossHealthComponentName);
        if (healthComponent != null)
        {
            System.Reflection.PropertyInfo maxHealthProperty =
                healthComponent.GetType().GetProperty("MaxHealth") ??
                healthComponent.GetType().GetProperty("maxHealth");

            System.Reflection.PropertyInfo currentHealthProperty =
                healthComponent.GetType().GetProperty(bossHealthPropertyName);

            if (maxHealthProperty != null)
            {
                bossMaxHealth = (int)maxHealthProperty.GetValue(healthComponent);
            }

            if (currentHealthProperty != null)
            {
                bossCurrentHealth = (int)currentHealthProperty.GetValue(healthComponent);
            }
        }
    }

    private void MonitorBossHealth()
    {
        Component healthComponent = currentBoss.GetComponent(bossHealthComponentName);
        if (healthComponent == null) return;

        System.Reflection.PropertyInfo healthProperty =
            healthComponent.GetType().GetProperty(bossHealthPropertyName);

        if (healthProperty == null) return;

        bossCurrentHealth = (int)healthProperty.GetValue(healthComponent);

        //if (bossHealthSlider != null)
        //{
        //    bossHealthSlider.value = bossCurrentHealth;
        //}

        if (bossCurrentHealth <= 0 && !bossDefeated)
        {
            OnBossDefeated();
        }
    }

    private void OnBossDefeated()
    {
        bossDefeated = true;
        Debug.Log($"{bossName} is dead");

        //if (bossHealthBar != null)
        //{
        //    bossHealthBar.SetActive(false);
        //}

        //if (audioSource != null && bossDefeatedSound != null)
        //{
        //    audioSource.PlayOneShot(bossDefeatedSound);
        //}

        if (timerController != null)
        {
            timerController.TogglePause();
        }

        StartCoroutine(ShowVictoryAfterDelay());

        if (monsterRespawnManager != null)
        {
            var eventField = monsterRespawnManager.GetType().GetField("OnMonsterDestroyed",
                System.Reflection.BindingFlags.Public |
                System.Reflection.BindingFlags.Instance);

            if (eventField != null)
            {
                var onMonsterDestroyed = eventField.GetValue(monsterRespawnManager) as System.Action<GameObject>;
                onMonsterDestroyed?.Invoke(currentBoss);
            }
        }
    }

    private IEnumerator ShowVictoryAfterDelay()
    {
        yield return new WaitForSeconds(delayBeforeVictory);

        WinningPageShown.Instance.ShowGameOver();
    }

    private IEnumerator AnimateWarning()
    {
        float blinkSpeed = 2.0f; // 闪烁速度
        float elapsedTime = 0;

        if (warningText == null)
        {
            yield return new WaitForSeconds(bossWarningTime);
            yield break;
        }

        Color originalColor = warningText.color;

        while (GetGameTime() < bossSpawnTime)
        {
            elapsedTime += Time.deltaTime;

            float alpha = Mathf.Abs(Mathf.Sin(elapsedTime * blinkSpeed));

            warningText.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);

            int secondsLeft = Mathf.CeilToInt(bossSpawnTime - GetGameTime());
            warningText.text = $"{bossName} will be here after {secondsLeft}s";

            yield return null;
        }

        warningText.color = originalColor;
    }

    private void EnableAllColliders(GameObject gameObject)
    {
        Collider[] colliders = gameObject.GetComponents<Collider>();
        foreach (var collider in colliders)
        {
            collider.enabled = true;
        }

        Collider2D[] colliders2D = gameObject.GetComponents<Collider2D>();
        foreach (var collider in colliders2D)
        {
            collider.enabled = true;
        }

        foreach (Transform child in gameObject.transform)
        {
            EnableAllColliders(child.gameObject);
        }
    }

    public void ForceSpawnBoss()
    {
        if (!bossSpawned)
        {
            SpawnBoss();
            bossSpawned = true;
        }
    }

    public void Reset()
    {
        bossSpawned = false;
        warningShown = false;
        bossDefeated = false;

        if (currentBoss != null)
        {
            Destroy(currentBoss);
            currentBoss = null;
        }

        if (bossWarningPanel != null)
            bossWarningPanel.SetActive(false);

        //if (bossHealthBar != null)
        //    bossHealthBar.SetActive(false);
    }
}