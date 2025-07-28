using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Player & Spawning")]
    public PlayerController player;
    public ObstacleSpawner spawner;

    [Header("UI")]
    public GameObject startMenu;
    public GameObject gameOverMenu;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;

    [Header("Score")]
    public float pointsPerSecond = 10f;

    private float score = 0f;
    private float highScore = 0f;
    public bool GameIsRunning { get; private set; } = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        Time.timeScale = 0f; // Juego detenido al inicio
        GameIsRunning = false;
        UpdateUI();
    }

    private void Update()
    {
        if (GameIsRunning)
        {
            score += pointsPerSecond * Time.deltaTime;
            UpdateUI();
        }
    }

    public void StartGame()
    {
        Time.timeScale = 1f;
        GameIsRunning = true;
        startMenu.SetActive(false);
        score = 0f;
    }

    public void GameOver()
    {
        GameIsRunning = false;
        Time.timeScale = 0f;
        gameOverMenu.SetActive(true);

        if (score > highScore)
            highScore = score;

        UpdateUI();
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void UpdateUI()
    {
        scoreText.text = $"Score: {(int)score}";
        highScoreText.text = $"High Score: {(int)highScore}";
    }

    public void AddScore(float amount)
    {
        score += amount;
        UpdateUI();
    }
}
