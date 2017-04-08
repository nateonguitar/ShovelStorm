using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MapManager : MonoBehaviour {
    public int neighborhood;
    Text neighborhoodText;
    Text moneyText;

    GameObject backANeighborhoodButton;
    Text backANeighborhoodText;
    GameObject forwardANeighborhoodButton;
    Text forwardANeighborhoodText;

    string neighborhoodForwardName;
    string neighborhoodBackName;

    GameObject[] levelIcons;
    int unlockedLevel;
    int unlockedNeighborhood;

    void Start()
    {
        neighborhoodForwardName = "Neighborhood" + (neighborhood + 1).ToString().PadLeft(3, '0');
        neighborhoodBackName = "Neighborhood" + (neighborhood - 1).ToString().PadLeft(3, '0');

        neighborhoodText = GameObject.FindWithTag("MapNeighborhoodText").GetComponent<Text>();
        moneyText = GameObject.FindWithTag("MapMoneyText").GetComponent<Text>();

        backANeighborhoodText = GameObject.FindWithTag("MapBackANeighborhoodText").GetComponent<Text>();
        backANeighborhoodButton = GameObject.FindWithTag("MapBackANeighborhoodButton");
        forwardANeighborhoodText = GameObject.FindWithTag("MapForwardANeighborhoodText").GetComponent<Text>();
        forwardANeighborhoodButton = GameObject.FindWithTag("MapForwardANeighborhoodButton");
        

        // set the back and forward neighborhood buttons up
        backANeighborhoodText.text = "Neighborhood " + (neighborhood - 1).ToString();
        forwardANeighborhoodText.text = "Neighborhood " + (neighborhood + 1).ToString();

        // don't show "back a neighborhood" if we're on the first neighborhood
        if(neighborhood == 1)
        {
            backANeighborhoodButton.SetActive(false);
        }
        else // else is to prevent this code from messing up on loading other
             // as this script is shard
        {
            backANeighborhoodButton.SetActive(true);
        }

        if (!Application.CanStreamedLevelBeLoaded(neighborhoodForwardName))
        {
            forwardANeighborhoodButton.SetActive(false);
        }
        else
        {
            forwardANeighborhoodButton.SetActive(true);
        }

        moneyText.text = "$" + PlayerPrefs.GetInt("money");
        neighborhoodText.text = "Neighborhood " + neighborhood.ToString();

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
            // if a level is before the unlocked neighborhood leve of course they're all unlocked
            if (levelIcons[i].GetComponent<MapLevelIcon>().neighborhood < unlockedNeighborhood)
            {
                levelIcons[i].transform.FindChild("Lock").gameObject.SetActive(false);
            }

            // only need to worry about unlocking part of them if the unlock neighborhood is equal
            // to the currently unlocked neighborhood level
            // if this is the case, unlock only the ones up to the allowed level
            else if (levelIcons[i].GetComponent<MapLevelIcon>().neighborhood == unlockedNeighborhood
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

    public void attemptToLoadForwardANeighborhood()
    {
        if (Application.CanStreamedLevelBeLoaded(neighborhoodForwardName))
        {
            SceneManager.LoadScene(neighborhoodForwardName);
        }
    }

    public void attemptToLoadBackANeighborhood()
    {
        
        
        if (Application.CanStreamedLevelBeLoaded(neighborhoodBackName))
        {
            SceneManager.LoadScene(neighborhoodBackName);
        }
    }
}
