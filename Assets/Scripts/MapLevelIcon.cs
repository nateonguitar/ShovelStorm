﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapLevelIcon: MonoBehaviour {
    public int level = 1;
    public int neighborhood = 1;
    [Range(0, 1)]
    public int finalLevelInThisNeighborhood = 0;

    void Start()
    {
        // adding an click event listener
        // when this gameobject is clicked, run handleClick()
        gameObject.GetComponent<Button>().onClick.AddListener(() => handleClick());
    }

    void handleClick()
    {
        PlayerPrefs.SetInt("levelChosenFromMap", level);
        PlayerPrefs.SetInt("neighborhoodChosenFromMap", neighborhood);
        GameObject.FindGameObjectWithTag("MapManager").GetComponent<MapManager>().attemptToStartLevel(level, neighborhood, finalLevelInThisNeighborhood);
    }
}
