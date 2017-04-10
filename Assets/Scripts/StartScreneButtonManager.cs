using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreneButtonManager : MonoBehaviour {
    public GameObject confirmNewGamePanel;
    public AudioSource backgroundMusic;



    void Awake()
    {
        /*
            This fixes a major issue:
            //When the game starts it plays the background music,
            but when you come back to the start screen mid game
            it would play the background music AGAIN stacked
            on top of the original background music.
            //Coming from the map to the start screen repeatedly would stack
            multiple songs on top of eachother.
            //The fix is to only play on the first load and destroy
            any that are made after.
            
        */
        GameObject[] menuBackgroundMusicsAlreadyPlaying = GameObject.FindGameObjectsWithTag("MenuBackgroundMusic");
        if (menuBackgroundMusicsAlreadyPlaying.Length > 1)
        {
            for(int i=1; i< menuBackgroundMusicsAlreadyPlaying.Length; i++)
            {
                Destroy(GameObject.FindGameObjectsWithTag("MenuBackgroundMusic")[i]);
            }
        }
        else
        {
            DontDestroyOnLoad(backgroundMusic);
            backgroundMusic.loop = true;

            backgroundMusic.Play();
        }
    }


    void Start()
    {
        Debug.Log("Number of background musics playing: " + GameObject.FindGameObjectsWithTag("MenuBackgroundMusic").Length.ToString());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void StartNewGame()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        PlayerPrefs.SetInt("difficultyLevel", 1);
        PlayerPrefs.SetInt("shovelLevel", 1);
        PlayerPrefs.SetInt("coatLevel", 1);
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
        SceneManager.LoadScene("Neighborhood" + PlayerPrefs.GetInt("unlockedNeighborhood").ToString().PadLeft(3, '0'));
    }

    public void endGame()
    {
        Application.Quit();
    }
}
