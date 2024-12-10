using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverManager : MonoBehaviour
{
    public GameObject gameOverPanel;
    public TextMeshProUGUI elapsedTimeText;
    private float startTime;

    private void Start()
    {
        startTime = Time.time;
        gameOverPanel.SetActive(false);
    }

    public void GameOver()
    {
        float elapsedTime = Time.time - startTime;
        string minutes = ((int)elapsedTime / 60).ToString();
        string seconds = (elapsedTime % 60).ToString("f2");

        elapsedTimeText.text = "Time: " + minutes + ":" + seconds;
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f; // ���� �Ͻ�����
    }

    public void Retry()
    {
        Time.timeScale = 1f; // ���� �簳
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); 
    }

    public void Quit()
    {
        Application.Quit(); // ���� ����
    }
}
