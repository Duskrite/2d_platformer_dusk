using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGameUIController : MonoBehaviour
{
    public int currentScore;
    public int maxScore = 100;
    public Slider progressBar;

    public GameObject defeatPanel;
    public GameObject victoryPanel;
    public GameObject inGamePanel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentScore = 0;
        progressBar.value = 0;
        ScoreItem.OnScoreItemCollected += UpdateProgress;
    }

    public void UpdateProgress(int score)
    {
        currentScore += score;
        if (currentScore >= maxScore)
        {
            currentScore = maxScore;
            Debug.Log("Maximum score reached!");
        }

        progressBar.value = currentScore;
    }

    public void Defeat()
    {

        Time.timeScale = 0f;
        inGamePanel.SetActive(false);
        defeatPanel.SetActive(true);
    }

    public void Victory()
    {
        Time.timeScale = 0f;
        inGamePanel.SetActive(false);
        victoryPanel.SetActive(true);
    }

    public void OnRestartClick()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void OnExistClick()
    {
        Application.Quit();
    }
}
