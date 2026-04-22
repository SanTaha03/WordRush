using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI livesText;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private GameObject gameOverText;
    [SerializeField] private GameObject finalScoreTextObject;
    [SerializeField] private TextMeshProUGUI finalScoreText;

    [Header("Game Settings")]
    [SerializeField] private int startingLives = 3;
    [SerializeField] private float gameDuration = 120f;
    [SerializeField] private SpawnEnemy spawnEnemy;
    [SerializeField] private WordInputManager wordInputManager;

    private int score = 0;
    private int currentLives;
    private float currentTime;
    private bool isGameOver = false;
    private bool isTimeOver = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        currentLives = startingLives;
        currentTime = gameDuration;

        UpdateScoreUI();
        UpdateLivesUI();
        UpdateTimerUI();

        if (gameOverText != null)
        {
            gameOverText.SetActive(false);
        }

        if (finalScoreTextObject != null)
        {
            finalScoreTextObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (isGameOver || isTimeOver)
        {
            return;
        }

        currentTime -= Time.deltaTime;

        if (currentTime < 0f)
        {
            currentTime = 0f;
        }

        UpdateTimerUI();

        if (currentTime <= 0f)
        {
            TriggerTimeOver();
        }
    }

    public void AddScore(int amount)
    {
        if (isGameOver || isTimeOver)
        {
            return;
        }

        score += amount;
        UpdateScoreUI();
        Debug.Log("Score actuel : " + score);
    }

    public void LoseLife(int amount = 1)
    {
        if (isGameOver || isTimeOver)
        {
            return;
        }

        currentLives -= amount;

        if (currentLives < 0)
        {
            currentLives = 0;
        }

        UpdateLivesUI();
        Debug.Log("Vies restantes : " + currentLives);

        if (currentLives <= 0)
        {
            TriggerGameOver();
        }
    }

    private void TriggerGameOver()
    {
        isGameOver = true;

        Debug.Log("GAME OVER");

        if (gameOverText != null)
        {
            gameOverText.SetActive(true);
        }

        ShowFinalScore();
        StopGameplay();
    }

    private void TriggerTimeOver()
    {
        isTimeOver = true;

        Debug.Log("FIN DU TEMPS");

        ShowFinalScore();
        StopGameplay();
    }

    private void StopGameplay()
    {
        if (spawnEnemy != null)
        {
            spawnEnemy.StopSpawning();
        }

        if (wordInputManager != null)
        {
            wordInputManager.SetInputEnabled(false);
        }

        Time.timeScale = 0f;
    }

    private void ShowFinalScore()
    {
        if (finalScoreTextObject != null)
        {
            finalScoreTextObject.SetActive(true);
        }

        if (finalScoreText != null)
        {
            finalScoreText.text = "SCORE FINAL : " + score;
        }
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "SCORE : " + score;
        }
    }

    private void UpdateLivesUI()
    {
        if (livesText != null)
        {
            livesText.text = "VIES : " + currentLives;
        }
    }

    private void UpdateTimerUI()
    {
        if (timerText != null)
        {
            int seconds = Mathf.CeilToInt(currentTime);
            timerText.text = "TEMPS : " + seconds;
        }
    }

    public bool IsGameOver()
    {
        return isGameOver;
    }

    public bool IsTimeOver()
    {
        return isTimeOver;
    }

    public int GetScore()
    {
        return score;
    }

    public int GetLives()
    {
        return currentLives;
    }
}