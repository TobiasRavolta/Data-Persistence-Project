using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public Text HighScoreText;
    public GameObject GameOverText;

    public static string highScoreHolder = "";
    public static int highScore = 0;
    public static string playerName;

    private bool m_Started = false;
    private int m_Points;

    private bool m_GameOver = false;

    
    // Start is called before the first frame update
    void Start()
    {
        LoadHighScore();
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }

        if (Input.GetKeyDown(KeyCode.Backspace) && m_GameOver)
        {
            m_GameOver = false;
            BackToMenu();
        }

        if (m_Points > highScore)
        {
            UpdateHighScore(playerName, m_Points);
            SaveHighScore();
        }

        if (highScoreHolder != null && HighScoreText != null)
        {
            HighScoreText.text = "High Score : " + highScoreHolder + " : " + highScore;
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);
    }

    public void BackToMenu()
    {
        playerName = null;
        SceneManager.LoadScene(0);
    }

    public void UpdateHighScore(string name, int score)
    {
        highScoreHolder = name;
        highScore = score;
    }

    [System.Serializable]
    class SaveData
    {
        public int _highScore;
        public string _highScoreHolder;
    }

    public void SaveHighScore()
    {
        SaveData data = new SaveData();
        data._highScore = highScore;
        data._highScoreHolder = highScoreHolder;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void LoadHighScore()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            highScore = data._highScore;
            highScoreHolder = data._highScoreHolder;
        }
    }
}
