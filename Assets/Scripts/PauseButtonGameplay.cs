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
            pauseWindowPanel.SetActive(true);
            pauseWindowShown = true;
            Time.timeScale = 0f;
            gamePlayController.gamePaused = true;
        }
        else
        {
            pauseWindowPanel.SetActive(false);
            pauseWindowShown = false;
            Time.timeScale = 1f;
            gamePlayController.gamePaused = false;
        }
    }

    public void backToMap()
    {
        SceneManager.LoadScene("LevelSelectMap");
    }

    public void quitGame()
    {
        SceneManager.LoadScene("StartMenu");
    }
}
