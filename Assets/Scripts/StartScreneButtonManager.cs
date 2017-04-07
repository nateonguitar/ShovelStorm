using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreneButtonManager : MonoBehaviour {
    public void StartNewGame()
    {
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

    public void Continue()
    {
        SceneManager.LoadScene("LevelSelectMap");
    }
}
