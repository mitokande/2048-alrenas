using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    private int score;
    private int highScore;
    public GridManager gridManager;
    public TextMeshProUGUI scoreText;
    public GameObject gameOverScreen;
    public TextMeshProUGUI gameOverScore;
    // Start is called before the first frame update
    void Start()
    {
        highScore = PlayerPrefs.GetInt("highScore",0);
        gameOverScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (highScore < score)
        {
            highScore = score;
            
        }
        scoreText.text = "Your Score: " + score + " High Score: " + highScore;
    }
    public void AddScore(int increment)
    {
        this.score += increment;
    }
    public void GameOver()
    {
        PlayerPrefs.SetInt("highScore", highScore);
        gridManager.gameObject.SetActive(false);
        gameOverScore.text = "YOu Have Lost! \nYour Score: " + score + " High Score: " + highScore;
        gameOverScreen.SetActive(true);
    }
    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
