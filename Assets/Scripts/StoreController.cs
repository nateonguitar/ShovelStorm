using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StoreController : MonoBehaviour {
    public Text currentMoneyText;

    public Text snowShovelIncreaseCost;
    public Text currentSnowShovelLevelText;

    public Text coatIncreaseCost;
    public Text currentCoatLevelText;


    int currentSnowShovelLevel;
    int nextSnowShovelLevelCost;
    int currentCoatLevel;
    int nextCoatLevelCost;

    void Start()
    {
        PlayerPrefs.SetInt("money", 100000);
        currentSnowShovelLevel = PlayerPrefs.GetInt("shovelLevel");
        currentCoatLevel = PlayerPrefs.GetInt("coatLevel");

        nextSnowShovelLevelCost = currentSnowShovelLevel * currentSnowShovelLevel * 100;
        nextCoatLevelCost = currentCoatLevel * currentCoatLevel * 200;

        // set the text for how much the next level costs
        snowShovelIncreaseCost.text = "$" + nextSnowShovelLevelCost.ToString();
        currentSnowShovelLevelText.text = "Level\n" + currentSnowShovelLevel;
        currentMoneyText.text = "You have\n$" + PlayerPrefs.GetInt("money").ToString();
        coatIncreaseCost.text = "$" + nextCoatLevelCost.ToString();
        currentCoatLevelText.text = "Level\n" + currentCoatLevel;

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Neighborhood" + PlayerPrefs.GetInt("unlockedNeighborhood").ToString().PadLeft(3, '0'));
        }
    }

    public void increaseShovelLevel()
    {
        Debug.Log("Next snow shovel cost: " + nextSnowShovelLevelCost);
        if(PlayerPrefs.GetInt("money") >= nextSnowShovelLevelCost)
        {
            PlayerPrefs.SetInt("money", PlayerPrefs.GetInt("money") - nextSnowShovelLevelCost);
            currentSnowShovelLevel++;
            nextSnowShovelLevelCost = currentSnowShovelLevel * currentSnowShovelLevel * 100;
            PlayerPrefs.SetInt("shovelLevel", currentSnowShovelLevel);
            snowShovelIncreaseCost.text = "$" + nextSnowShovelLevelCost.ToString();
            currentMoneyText.text = "You have\n$" + PlayerPrefs.GetInt("money").ToString();
            currentSnowShovelLevelText.text = "Level\n" + currentSnowShovelLevel;
        }
        Debug.Log("Amount of money the player has: " + PlayerPrefs.GetInt("money").ToString());
    }

    public void increaseCoatLevel()
    {
        Debug.Log("Next coat cost: " + nextSnowShovelLevelCost);
        if (PlayerPrefs.GetInt("money") >= nextCoatLevelCost)
        {
            PlayerPrefs.SetInt("money", PlayerPrefs.GetInt("money") - nextCoatLevelCost);
            currentCoatLevel++;
            nextCoatLevelCost = currentCoatLevel * currentCoatLevel * 200;
            PlayerPrefs.SetInt("coatLevel", currentCoatLevel);
            coatIncreaseCost.text = "$" + nextCoatLevelCost.ToString();
            currentMoneyText.text = "You have\n$" + PlayerPrefs.GetInt("money").ToString();
            currentCoatLevelText.text = "Level\n" + currentCoatLevel;
        }
        Debug.Log("Amount of money the player has: " + PlayerPrefs.GetInt("money").ToString());
    }

    public void backToMap()
    {
        SceneManager.LoadScene("Neighborhood" + PlayerPrefs.GetInt("unlockedNeighborhood").ToString().PadLeft(3, '0'));
    }
}
