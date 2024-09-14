using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;
    public Text ScoreText;
    public GameObject GameOverText;
    public Text highscoresText;
    public Text CurrentplayerName;

    private bool m_Started = false;
    private int m_Points;
    private int m_TotalBrick = 0;
    private bool m_GameOver = false;

    void Start()
    {
        InitiateBlocks();
        UpdateHighscoresUI();
        CurrentPlayerNameSet();
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

    private void InitiateBlocks()
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
                brick.onDestroyed.AddListener(OnBrickDestroy);
                m_TotalBrick++;
            }
        }
    }

    private void OnBrickDestroy(int point)
    {
        AddPoint(point);
        m_TotalBrick--;

        if (m_TotalBrick <= 0)
        {
            InitiateBlocks();
        }
    }

    private void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
    }

    private void StartGame()
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
        
        if (DataManager.Instance != null)
        {
            DataManager.Instance.AddOrUpdateHighscore(DataManager.Instance.currentPlayerId, m_Points);
            UpdateHighscoresUI();
        }
    }

    private void UpdateHighscoresUI()
    {
        if (highscoresText != null && DataManager.Instance != null)
        {
            highscoresText.text = DataManager.Instance.GetFormattedHighscores();
        }
    }

    private void CurrentPlayerNameSet()
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