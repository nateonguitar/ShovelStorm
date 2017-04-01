﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreneTouchManager : MonoBehaviour {

    GameObject gameObjectThatWasTouched = null;

    // set in the editor
   // public Text displayTouchesText;

    void Update()
    {
        if (Input.touchCount > 0)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                Ray mouseRay = GenerateMouseRay(Input.GetTouch(0).position);
                RaycastHit hit;

                if (Physics.Raycast(mouseRay.origin, mouseRay.direction, out hit))
                {
                    gameObjectThatWasTouched = hit.transform.gameObject;
                    Debug.Log(gameObjectThatWasTouched.tag);
                    //displayTouchesText.text = "You touched: " + gameObjectThatWasTouched.tag;
                    if(gameObjectThatWasTouched.tag == "NewGameButton")
                    {
                        PlayerPrefs.SetInt("difficultyLevel", 1);
                        PlayerPrefs.SetInt("shovelLevel", 1);
                        PlayerPrefs.SetInt("unlockedLevel", 3);

                        PlayerPrefs.Save();

                        SceneManager.LoadScene("NewGameVideo");
                    }

                    if (gameObjectThatWasTouched.tag == "ContinueButton")
                    {
                        SceneManager.LoadScene("LevelSelectMap");
                    }
                }
                else
                {
                    //displayTouchesText.text = "You touched the screen, but nothing hit";
                }
            }
        }
    }

    Ray GenerateMouseRay(Vector3 touchPos)
    {
        Vector3 mousePosFar = new Vector3(touchPos.x, touchPos.y, Camera.main.farClipPlane);
        Vector3 mousePosNear = new Vector3(touchPos.x, touchPos.y, Camera.main.nearClipPlane);
        Vector3 mousePosF = Camera.main.ScreenToWorldPoint(mousePosFar);
        Vector3 mousePosN = Camera.main.ScreenToWorldPoint(mousePosNear);

        Ray mr = new Ray(mousePosN, mousePosF - mousePosN);
        return mr;
    }
}