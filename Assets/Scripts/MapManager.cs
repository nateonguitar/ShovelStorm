using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapManager : MonoBehaviour {
    GameObject gameObjectThatWasTouched = null;
    public List<GameObject> Levels = new List<GameObject>();
    int unlockedLevel;

    // set in the editor
    // public Text displayTouchesText;

    void Start()
    {
        unlockedLevel = PlayerPrefs.GetInt("unlockedLevel");

        for(int i=1; i<= unlockedLevel; i++)
        {
            GameObject.FindWithTag("Level" + i.ToString().PadLeft(3, '0')).transform.FindChild("Lock").gameObject.SetActive(false);
        }
    }
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

                    if(gameObjectThatWasTouched.tag.Substring(0, 5) == "Level")
                    {
                        int difficulty = int.Parse(gameObjectThatWasTouched.tag.Substring(5, 3));
                        PlayerPrefs.SetInt("difficultyLevel", difficulty);
                        SceneManager.LoadScene("GamePlayLevel");
                    }
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
