using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreneButtonManager : MonoBehaviour {
    public GameObject confirmNewGamePanel;

    public void StartNewGame()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        PlayerPrefs.SetInt("difficultyLevel", 1);
        PlayerPrefs.SetInt("shovelLevel", 1);
        PlayerPrefs.SetInt("unlockedNeighborhood", 1);
        PlayerPrefs.SetInt("unlockedLevel", 1);
        PlayerPrefs.SetInt("playingFinalLevelInNeighborhood", 0);
        PlayerPrefs.SetInt("levelChosenFromMap", 1);
        PlayerPrefs.SetInt("neighborhoodChosenFromMap", 1);
        PlayerPrefs.SetInt("money", 0);

        PlayerPrefs.Save();

        SceneManager.LoadScene("NewGameVideo");
    }

    public void showConfirmStartNewGame()
    {
        confirmNewGamePanel.SetActive(true);
    }

    public void hideConfirmStartNewGame()
    {
        confirmNewGamePanel.SetActive(false);
    }

    public void Continue()
    {
        // this is a way to test if the playerprefs are in existence
        if(PlayerPrefs.GetInt("difficultyLevel") == 0)
        {
            return;
        }
        SceneManager.LoadScene("LevelSelectMap");
    }

    public void endGame()
    {
        Application.Quit();
    }
}
