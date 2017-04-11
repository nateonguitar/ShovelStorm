using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameplayMenus : MonoBehaviour {

    public GameObject pauseWindowPanel;
    public GameObject winLosePanel;
    public GameObject winLosePanelDrivewayCleared;
    public GameObject winLosePanelYouFrozeToDeath;
    public Text winLosePanelMoneyEarned;
    public GameObject winLosePanelBackToMap;
    public GameObject winLosePanelRetry;
    public GameObject winLosePanelContinue;

    GamePlayController gamePlayController;
    MusicManager musicManager;
    bool pauseWindowShown = false;
    int money = 0;

    Vector3 winLosePanelMoneyEarnedOriginalSize;

    void Start()
    {
        pauseWindowPanel.SetActive(false);
        winLosePanelDrivewayCleared.SetActive(false);
        winLosePanelYouFrozeToDeath.SetActive(false);
        winLosePanelBackToMap.SetActive(false);
        winLosePanelRetry.SetActive(false);
        winLosePanelContinue.SetActive(false);
        winLosePanel.SetActive(false);

        winLosePanelMoneyEarnedOriginalSize = winLosePanelMoneyEarned.transform.localScale;

        gamePlayController = GameObject.FindWithTag("GamePlayController").GetComponent<GamePlayController>();
        musicManager = GameObject.FindWithTag("GamePlayController").GetComponent<MusicManager>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            onPauseButtonTouch();
        }
    }

    public void hideMenusForGameStart()
    {
        winLosePanel.SetActive(false);
    }
    public void showDrivewayCleared()
    {
        winLosePanel.SetActive(true);
        winLosePanelDrivewayCleared.SetActive(true);
        winLosePanelYouFrozeToDeath.SetActive(false);
        winLosePanelBackToMap.SetActive(false);
        winLosePanelRetry.SetActive(false);
        winLosePanelContinue.SetActive(true);
        setMoneyText(money);
    }

    public void showYouFrozeToDeath()
    {
        winLosePanel.SetActive(true);
        winLosePanelDrivewayCleared.SetActive(false);
        winLosePanelYouFrozeToDeath.SetActive(true);
        winLosePanelBackToMap.SetActive(true);
        winLosePanelRetry.SetActive(true);
        winLosePanelContinue.SetActive(false);
        setMoneyText(0);
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
        musicManager.startMenuMusicBackUp();
        SceneManager.LoadScene("Neighborhood" + PlayerPrefs.GetInt("unlockedNeighborhood").ToString().PadLeft(3, '0'));
        
    }

    public void backToStartMenu()
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

    private void setMoneyText(int money)
    {
        if(money != 0)
        {
            winLosePanelMoneyEarned.transform.localScale = winLosePanelMoneyEarnedOriginalSize;
            winLosePanelMoneyEarned.text = "You Earned\n$" + money;
        }
        else
        {
            winLosePanelMoneyEarned.transform.localScale = Vector3.zero;
        }
    }

    public void setMoney(int money)
    {
        this.money = money;
    }

    public void reloadThisLevel()
    {
        SceneManager.LoadScene("GamePlayLevel");
    }

    public void unlockNextLevelAndSendBackToMap()
    {
        int unlockedNeighborhood = PlayerPrefs.GetInt("unlockedNeighborhood");
        int unlockedLevel = PlayerPrefs.GetInt("unlockedLevel");
        int playingFinalLevelInNeighborhood = PlayerPrefs.GetInt("playingFinalLevelInNeighborhood");
        int levelChosenFromMap = PlayerPrefs.GetInt("levelChosenFromMap");

        // if they beat the final level in the neighborhood allow the next
        if (playingFinalLevelInNeighborhood == 1)
        {
            unlockedNeighborhood++;
            unlockedLevel = 1;
        }
        else
        {
            if(levelChosenFromMap == unlockedLevel)
            {
                unlockedLevel++;
            }
        }

        PlayerPrefs.SetInt("unlockedNeighborhood", unlockedNeighborhood);
        PlayerPrefs.SetInt("unlockedLevel", unlockedLevel);
        PlayerPrefs.SetInt("playingFinalLevelInNeighborhood", 0);
        PlayerPrefs.Save();
        musicManager.startMenuMusicBackUp();
        SceneManager.LoadScene("Neighborhood" + PlayerPrefs.GetInt("unlockedNeighborhood").ToString().PadLeft(3, '0'));
        

    }
}
