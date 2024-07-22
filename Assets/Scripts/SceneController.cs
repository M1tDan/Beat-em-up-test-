using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public GameObject pausedText;

    public void LoadGame()
    {
        SceneManager.LoadScene(1);
    }
    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void GamePause()
    {
        if (Time.timeScale == 1f)
        {
            Time.timeScale = 0f;
            pausedText.SetActive(true);
        }
        else
        {
            Time.timeScale = 1f;
            pausedText.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GamePause();
        }
    }
}