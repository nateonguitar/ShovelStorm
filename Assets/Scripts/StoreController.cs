using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StoreController : MonoBehaviour {
    public Text snowShovelIncreaseCost;
    public Text currentMoneyText;
    public Text currentSnowShovelLevelText;
    int currentSnowShovelLevel;
    int nextSnowShovelLevelCost; 

    void Start()
    {
        PlayerPrefs.SetInt("money", 10000);
        PlayerPrefs.SetInt("shovelLevel", 1);
        PlayerPrefs.Save();

        currentSnowShovelLevel = PlayerPrefs.GetInt("shovelLevel");

        nextSnowShovelLevelCost = currentSnowShovelLevel * currentSnowShovelLevel * 100;

        // set the text for how much the next level costs
        snowShovelIncreaseCost.text = "$" + nextSnowShovelLevelCost.ToString();
        currentMoneyText.text = "You have\n$" + PlayerPrefs.GetInt("money").ToString();
        currentSnowShovelLevelText.text = "Level\n" + currentSnowShovelLevel;
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

    public void backToMap()
    {
        SceneManager.LoadScene("Neighborhood" + PlayerPrefs.GetInt("unlockedNeighborhood").ToString().PadLeft(3, '0'));
    }
}
