using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseButtonGameplay : MonoBehaviour {

    public GameObject pauseWindowPanel;
    GamePlayController gamePlayController;
    bool pauseWindowShown = false;

    void Start()
    {
        gamePlayController = GameObject.FindWithTag("GamePlayController").GetComponent<GamePlayController>();
    }

	public void onPauseButtonTouch()
    {
        if (!pauseWindowShown)
        {
            pause();
        }
        else
        {
            unPause();
        }
    }

    public void backToMap()
    {
        unPause();
        SceneManager.LoadScene("LevelSelectMap");
    }

    public void quitGame()
    {
        unPause();
        SceneManager.LoadScene("StartMenu");
    }

    private void pause()
    {
        pauseWindowPanel.SetActive(true);
        pauseWindowShown = true;
        Time.timeScale = 0f;
        gamePlayController.gamePaused = true;
    }

    private void unPause()
    {
        pauseWindowPanel.SetActive(false);
        pauseWindowShown = false;
        Time.timeScale = 1f;
        gamePlayController.gamePaused = false;
    }
}
