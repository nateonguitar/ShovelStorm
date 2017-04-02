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
        Right = 3,
        None = 4
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

    
    void Update()
    {
        // swipeDirection will be Directions.None if no arrow keydown is detected
        Directions swipeDirection = checkForKeyboardArrowControls();

        //trigger a swipe based on the direction you pressed
        if (swipeDirection != Directions.None)
        {
            //Debug.Log(direction);
            swipe(swipeDirection);
            if (numberOfPlayerSwipesForTheCurrentTile == numberOfSwipesNeededToCompleteCurrentTile)
            {
                // build the next array of swipes
                // increment the cell the player is on
                // reset the freeze timer
                readyUpNextTile();
            }
        }

        // clear a row if you press C
        // for testing purposes only
        if (Input.GetKeyDown(KeyCode.C))
        {
            readyUpNextTile();
        }

        // touch input controls
        // if the number of touches on the screen is 1 or greater
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);


            // start detecting a swipe
            // this could also be used to detect a tap
            // just need to see if the distance between Began and Ended is tiny
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

                // if you swiped far enough and fast enough register a swipe
                if (swipeTime < maxTime && swipeDistance > minSwipeDist)
                {
                    swipe();

                    if (numberOfPlayerSwipesForTheCurrentTile == numberOfSwipesNeededToCompleteCurrentTile)
                    {
                        // build the next array of swipes
                        // increment the cell the player is on
                        // reset the freeze timer
                        readyUpNextTile();
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

    Directions checkForKeyboardArrowControls()
    {
        Directions swipeDirection = Directions.None;

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            swipeDirection = Directions.Up;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            swipeDirection = Directions.Down;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            swipeDirection = Directions.Right;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            swipeDirection = Directions.Left;
        }
        else
        {
            swipeDirection = Directions.None;
        }
        return swipeDirection;
    }

    void readyUpNextTile()
    {
        numberOfPlayerSwipesForTheCurrentTile = 0;
        playerSwipes.text = "";
        tilePlayerIsOn++;
        buildSwipes();
        freezeTimerController.ResetTimer();
    }

    void swipe(Directions passedInDirectionFromKeyboardControls = Directions.None)
    {
        // need to check if it is a vertical swipe or horizontal swipe first
        Vector2 distance = endPos - startPos;

        int swipeDirection = 0;

        // if you swiped
        if (passedInDirectionFromKeyboardControls == Directions.None)
        {
            Debug.Log("SwipeControls");
            if (Mathf.Abs(distance.x) > Mathf.Abs(distance.y))
            {
                if (distance.x > 0)
                {
                    swipeDirection = (int)Directions.Right;
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
        }
        else // if you used arrow controls to mock a swipe
        {
            swipeDirection = (int)passedInDirectionFromKeyboardControls;
            //Debug.Log(swipeDirection);
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
