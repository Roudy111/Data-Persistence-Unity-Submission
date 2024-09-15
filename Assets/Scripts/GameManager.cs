using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;
    public Text ScoreText;
    public Text HighscoreText;
    public GameObject GameOverText;
    public Text CurrentplayerName;

    private bool m_Started = false;
    private int m_TotalBrick = 0;
    private bool m_GameOver = false;

    private ScoreManager scoreManager;

    void Start()
    {
        scoreManager = ScoreManager.Instance;
        if (scoreManager == null)
        {
            Debug.LogError("ScoreManager not found!");
            return;
        }

        scoreManager.OnScoreChanged += UpdateScoreText;
        scoreManager.OnHighscoreUpdated += UpdateHighscoreText;
        scoreManager.ResetScore();

        InitiateBlocks();
        UpdateHighscoreText();
        CurrentPlayerNameSet();
    }

    void OnDestroy()
    {
        if (scoreManager != null)
        {
            scoreManager.OnScoreChanged -= UpdateScoreText;
            scoreManager.OnHighscoreUpdated -= UpdateHighscoreText;
        }
    }

    void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartGame();
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void InitiateBlocks()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        m_TotalBrick = 0;

        int[] pointCountArray = new[] { 1, 1, 2, 2, 5, 5 };
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
                m_TotalBrick++;
            }
        }
    }

    void AddPoint(int point)
    {
        scoreManager.AddPoints(point);
        m_TotalBrick--;

        if (m_TotalBrick <= 0)
        {
            InitiateBlocks();
        }
    }

    void UpdateScoreText(int score)
    {
        ScoreText.text = $"Score : {score}";
    }

    void UpdateHighscoreText()
    {
        if (HighscoreText != null && DataManager.Instance != null)
        {
            string highscores = DataManager.Instance.GetFormattedHighscores();
            HighscoreText.text = $"Highscores:\n{highscores}";
        }
    }

    void StartGame()
    {
        m_Started = true;
        float randomDirection = Random.Range(-1.0f, 1.0f);
        Vector3 forceDir = new Vector3(randomDirection, 1, 0);
        forceDir.Normalize();

        Ball.transform.SetParent(null);
        Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);
        UpdateHighscoreText();
    }

    void CurrentPlayerNameSet()
    {
        if (CurrentplayerName != null && DataManager.Instance != null)
        {
            CurrentplayerName.text = $"Player Name: {DataManager.Instance.currentPlayerId}";
        }
    }

    public void Back2Menu()
    {
        SceneManager.LoadScene(0);
    }
}