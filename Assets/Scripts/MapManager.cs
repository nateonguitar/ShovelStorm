using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MapManager : MonoBehaviour {
    public Text moneyText;

    GameObject[] levelIcons;
    int unlockedLevel;
    int unlockedNeighborhood;


    // set in the editor
    // public Text displayTouchesText;

    void Start()
    {
        moneyText.text = "$" + PlayerPrefs.GetInt("money");
        // fill our levelIcons with all of the level icons placed on the map in the editor
        levelIcons = GameObject.FindGameObjectsWithTag("MapLevelIcon");

        // get how far the player has unlocked
        unlockedLevel = PlayerPrefs.GetInt("unlockedLevel");
        unlockedNeighborhood = PlayerPrefs.GetInt("unlockedNeighborhood");

        // remove lock images from those that are available for the player
        // set the text value for each level
        for(int i=0; i < levelIcons.Length; i++)
        {
            // set the text
            levelIcons[i].transform.FindChild("Text").GetComponent<Text>().text = levelIcons[i].GetComponent<MapLevelIcon>().level.ToString();

            // remove the locks
            if (levelIcons[i].GetComponent<MapLevelIcon>().neighborhood <= unlockedNeighborhood
                && levelIcons[i].GetComponent<MapLevelIcon>().level <= unlockedLevel)
            {
                levelIcons[i].transform.FindChild("Lock").gameObject.SetActive(false);
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            backToStartMenu();
        }
    }
    
    public void attemptToStartLevel(int level, int neighborhood, int finalLevelInThisNeighborhood)
    {
        if(level <= unlockedLevel && neighborhood <= unlockedNeighborhood)
        {
            PlayerPrefs.SetInt("levelChosenFromMap", level);
            PlayerPrefs.SetInt("neighborhoodChosenFromMap", neighborhood);
            PlayerPrefs.SetInt("playingFinalLevelInNeighborhood", finalLevelInThisNeighborhood);
            SceneManager.LoadScene("GamePlayLevel", LoadSceneMode.Single);
        }
    }

    public void backToStartMenu()
    {
        SceneManager.LoadScene("StartMenu");
    }

    public void loadStoreScene()
    {
        SceneManager.LoadScene("Store");
    }

    public void loadStatusScene()
    {
        SceneManager.LoadScene("Status");
    }
}
