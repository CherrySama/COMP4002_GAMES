using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject deadImage; 
    [SerializeField] private Button restartButton; 

    [Header("Setting")]
    [SerializeField] private float restartDelay = 1.0f; 
    [SerializeField] private float timeScaleOnDeath = 0.5f; 

    private bool canRestart = false;
    private static GameManager instance;

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindFirstObjectByType<GameManager>();
                if (instance == null)
                {
                    GameObject go = new GameObject("GameOverManager");
                    instance = go.AddComponent<GameManager>();
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        if (deadImage == null)
        {
            deadImage = GameObject.Find("Dead_Image");
            if (deadImage != null)
            {
                deadImage.SetActive(false); 
            }
            else
            {
                Debug.LogWarning("cant find a Dead_Image");
            }
        }
        else
        {
            deadImage.SetActive(false); 
        }
        SetupRestartButton();
    }

    private void SetupRestartButton()
    {
        if (restartButton == null && deadImage != null)
        {
            restartButton = deadImage.GetComponentInChildren<Button>();

            if (restartButton == null)
            {
                Debug.LogWarning("cant find restartButton");
            }
        }

        // If the button is found, add a click event listener
        if (restartButton != null)
        {
            // Remove all existing listeners to prevent duplicate additions
            restartButton.onClick.RemoveAllListeners();

            restartButton.onClick.AddListener(RestartGame);

            restartButton.interactable = true;
        }
    }

    public void ShowGameOver()
    {
        // È·±£UI´æÔÚ
        if (deadImage != null)
        {
            deadImage.SetActive(true);
        }
        else
        {
            return;
        }

        Time.timeScale = timeScaleOnDeath;

        StartCoroutine(EnableRestartAfterDelay());
    }

    public void HideGameOver()
    {
        if (deadImage != null)
        {
            deadImage.SetActive(false);
        }

        Time.timeScale = 1.0f;
        canRestart = false;
    }

    private IEnumerator EnableRestartAfterDelay()
    {
        canRestart = false;

        if (restartButton != null)
        {
            restartButton.interactable = false;
        }

        yield return new WaitForSecondsRealtime(restartDelay);

        canRestart = true;

        Time.timeScale = 1.0f;

        if (restartButton != null)
        {
            restartButton.interactable = true;
        }
    }

    private void Update()
    {
        if (deadImage != null && deadImage.activeSelf && canRestart)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                RestartGame();
            }
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1.0f;

        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
}