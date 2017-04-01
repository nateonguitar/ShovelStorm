using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayController : MonoBehaviour {
    FreezeTimerController freezeTimerController;

    public Text swipesNeeded;
    public Text playerSwipes;

    public enum Directions
    {
        Up = 0,
        Down = 1,
        Left = 2,
        Right = 3
    }



    int[] swipesToClearThisTile = new int[100];
    int[] playerSwipesForThisTile = new int[100];
    int numberOfPlayerSwipesForTheCurrentTile = 0;
    int difficultyLevel;
    int shovelLevel;
    private bool tileFinished = false;
    public bool gameOver = false;
    private int numberOfSwipesNeededToCompleteCurrentTile;

    int tilePlayerIsOn = 0;

    public int numberOfTilesTilDifficultyIncrease = 3;

    public float maxTime = 0.5f;
    public float minSwipeDist = 50f;

    float startTime;
    float endTime;

    Vector3 startPos;
    Vector3 endPos;

    float swipeDistance;
    float swipeTime;

    void Start()
    {
        difficultyLevel = PlayerPrefs.GetInt("difficultyLevel");
        shovelLevel = PlayerPrefs.GetInt("shovelLevel");
        freezeTimerController = GameObject.FindWithTag("GamePlayController").GetComponent<FreezeTimerController>();
        buildSwipes();
        playerSwipes.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        if (tileFinished)
        {
            buildSwipes();
            clearRow();
            tileFinished = false;
        }

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                startTime = Time.time;
                startPos = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                endTime = Time.time;
                endPos = touch.position;

                swipeDistance = (endPos - startPos).magnitude;
                swipeTime = endTime - startTime;

                if (swipeTime < maxTime && swipeDistance > minSwipeDist)
                {
                    swipe();

                    if (numberOfPlayerSwipesForTheCurrentTile == numberOfSwipesNeededToCompleteCurrentTile)
                    {
                        numberOfPlayerSwipesForTheCurrentTile = 0;
                        playerSwipes.text = "";
                        tilePlayerIsOn++;
                        buildSwipes();
                        freezeTimerController.ResetTimer();
                    }
                }
            }
        }


        if (gameOver)
        {
            //Debug.Log("GameOver");
        }
    }

    void clearRow()
    {

    }

    void swipe()
    {
        // need to check if it is a vertical swipe or horizontal swipe first
        Vector2 distance = endPos - startPos;

        int swipeDirection = 0;

        if (Mathf.Abs(distance.x) > Mathf.Abs(distance.y))
        {
            if (distance.x > 0)
            {
                swipeDirection = (int) Directions.Right;
            }
            else if (distance.x < 0)
            {
                swipeDirection = (int)Directions.Left;
            }
        }
        else if (Mathf.Abs(distance.x) < Mathf.Abs(distance.y))
        {
            if (distance.y > 0)
            {
                swipeDirection = (int)Directions.Up;
            }
            else if (distance.y < 0)
            {
                swipeDirection = (int)Directions.Down;
            }
        }

        // if the player swipes the wrong direction
        if(swipesToClearThisTile[numberOfPlayerSwipesForTheCurrentTile] != swipeDirection)
        {
            // start their swiping over for this tile
            playerSwipes.text = "";
            numberOfPlayerSwipesForTheCurrentTile = 0;
        }
        else
        {
            
            playerSwipesForThisTile[numberOfPlayerSwipesForTheCurrentTile] = swipeDirection;

            if (swipeDirection == (int)Directions.Up)
            {
                playerSwipes.text += "Up ";
            }
            else if (swipeDirection == (int)Directions.Down)
            {
                playerSwipes.text += "Down ";
            }
            else if (swipeDirection == (int)Directions.Left)
            {
                playerSwipes.text += "Left ";
            }
            else if (swipeDirection == (int)Directions.Right)
            {
                playerSwipes.text += "Right ";
            }
            numberOfPlayerSwipesForTheCurrentTile++;
        }
        
    }

    // Use this for initialization
    

    private void buildSwipes()
    {
        
        swipesNeeded.text = "";
        
        numberOfSwipesNeededToCompleteCurrentTile = 3 + difficultyLevel - (shovelLevel * 2) + (tilePlayerIsOn / numberOfTilesTilDifficultyIncrease);
        Debug.Log("Tile player is on: " + tilePlayerIsOn);
        Debug.Log("Number of tiles til difficulty increases: " + numberOfTilesTilDifficultyIncrease);
        Debug.Log("tilePlayerIsOn / numberOfTilesTilDifficultyIncrease: " + tilePlayerIsOn / numberOfTilesTilDifficultyIncrease);

        // things break if set to < 1
        if (numberOfSwipesNeededToCompleteCurrentTile < 1)
        {
            numberOfSwipesNeededToCompleteCurrentTile = 1;
        }

        for(int i=0; i<numberOfSwipesNeededToCompleteCurrentTile; i++)
        {
            int swipe = (int)Mathf.Round(Random.Range(-0.51f, 3.49f));

            if (swipe == (int) Directions.Up)
            {
                swipesToClearThisTile[i] = (int) Directions.Up;
                swipesNeeded.text += "Up ";
            }
            else if (swipe == (int)Directions.Down)
            {
                swipesToClearThisTile[i] = (int)Directions.Down;
                swipesNeeded.text += "Down ";
            }
            else if (swipe == (int)Directions.Left)
            {
                swipesToClearThisTile[i] = (int)Directions.Left;
                swipesNeeded.text += "Left ";
            }
            else if (swipe == (int)Directions.Right)
            {
                swipesToClearThisTile[i] = (int)Directions.Right;
                swipesNeeded.text += "Right ";
            }
        }
    }
}
