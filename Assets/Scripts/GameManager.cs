using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.Scripting;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
   
    public GameObject GameOverText;
    public Text bestScoreText;
    public Text CurrentplayerName;
    
    private bool m_Started = false;
    private int m_Points;
    private int m_TotalBrick = 0;


    private bool m_GameOver = false;

    public int highScore = 0;
   






    // Start is called before the first frame update
    void Start()
    {
        InitiateBlocks();
        BestScoreUi();
        CurrentPlayerNameSet();    
        
       
    }


    private void Update()
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
            RestartGame();

        }


    }




    /// <summary>
    /// This is the main method for making the blocks of Bricks. 
    /// I put a m_totalbrick to check it later if it is 0 then needed to be initiated again for next level -- then it never stops
    /// </summary>
    public void InitiateBlocks()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        m_TotalBrick = 0;
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(onBrickDestroy);
                m_TotalBrick++;
            }
        }

    }
    

    private void onBrickDestroy(int point)
    {
        AddPoint(point);
        m_TotalBrick--;

        if (m_TotalBrick <= 0)
        {
            InitiateBlocks();
        }



    }
    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";

    }


    /// <summary>
    /// Here comes the states of the game when the game is blocks are intiated. 
    /// For more furthur customization, they need to have listiner and then be called.
    /// </summary>
    public void StartGame()
    {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
             


    }
    public void RestartGame()
    {
         if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        

    }

    public void BestScoreUi()
    {
        if (DataManager.Instance.HighScore < m_Points)
        {
            DataManager.Instance.HighScore = m_Points;

        }
        bestScoreText.text = $"BestScore : {DataManager.Instance.FirstPlayer} {DataManager.Instance.HighScore}";

    }

    public void Back2Menu()
    {
        SceneManager.LoadScene(0);
    } 


    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);
        BestScoreUi();
    }

    public void CurrentPlayerNameSet()
    {
        if (CurrentplayerName != null)
        {
            CurrentplayerName.text = $"Player Name : {DataManager.Instance.playerId}";

        }
        else
        {
            Debug.LogWarning("CurrentplayerName.text = $\"Player Name : {DataManager.Instance.playerId}");
        }
        
        


    }






}
