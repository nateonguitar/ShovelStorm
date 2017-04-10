using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayController : MonoBehaviour {
    public GameObject winLosePanel;

    FreezeTimerController freezeTimerController;
    GamePlayReadyStartAnimator gamePlayReadyStartAnimator;
    GameplayMenus gameplayMenus;
    

    public enum Directions
    {
        Up = 0,
        Down = 1,
        Left = 2,
        Right = 3,
        None = 4
    }
  

    int[] swipesToClearThisTile = new int[100];
    List<GameObject> swipesToClearThisTileArrows = new List<GameObject> ();
    public GameObject arrowPrefab;
    int[] playerSwipesForThisTile = new int[100];
    int numberOfPlayerSwipesForTheCurrentTile = 0;
    int levelChosenFromMap;
    int neighborhoodChosenFromMap;
    int difficultyLevel;
    int shovelLevel;
    int money;
    public bool gameOver = false;
    private int numberOfSwipesNeededToCompleteCurrentTile;

    int tilePlayerIsOn = 0;

    public int numberOfTilesTilDifficultyIncrease = 3;

    public float maxTime = 0.5f;
    public float minSwipeDist = 50f;


    public GameObject SnowMove;
    private float targetPoint;
    private Vector3 tileOriginalScale;

    public int baseNumberOfRows = 2;
    private int numberOfRowsOnThisLevel;

    float startTime;
    float endTime;

    Vector3 startPos;
    Vector3 endPos;

    float swipeDistance;
    float swipeTime;

    public bool gamePaused;
    private bool gameWon = false;
    private bool winLosePanelShown = false;


    void Start()
    {
        gameplayMenus = gameObject.GetComponent<GameplayMenus>();
        gameplayMenus.hideMenusForGameStart();
        // when restarting the level we need to make sure the game is not paused
        gamePaused = false;

        // get information from PlayerPrefs
        levelChosenFromMap = PlayerPrefs.GetInt("levelChosenFromMap");
        neighborhoodChosenFromMap = PlayerPrefs.GetInt("neighborhoodChosenFromMap");
        shovelLevel = PlayerPrefs.GetInt("shovelLevel");
        money = PlayerPrefs.GetInt("money");

        // calculate how the level should act
        difficultyLevel = levelChosenFromMap * neighborhoodChosenFromMap;
        numberOfRowsOnThisLevel = baseNumberOfRows + neighborhoodChosenFromMap;
        tileOriginalScale = SnowMove.transform.localScale;

        freezeTimerController = GameObject.FindWithTag("GamePlayController").GetComponent<FreezeTimerController>();
        freezeTimerController.ResetTimer();

        gamePlayReadyStartAnimator = GameObject.FindWithTag("GamePlayController").GetComponent<GamePlayReadyStartAnimator>();

        // display the Ready/Start at the beginning of the level
        gamePlayReadyStartAnimator.StartAnimation();

        Vector3 arrowsStartingPoint = new Vector3(-2.5f, 0.5f, 2.5f);
        float spaceBetweenEachArrow = 1.9f;
        int arrowsPerRow = 6;
        int numberOfRows = 3;

        for (int i = 0; i < numberOfRows; i++)
        {
            for (int j = 0; j < arrowsPerRow; j++)
            {
                swipesToClearThisTileArrows.Add(
                    Instantiate(
                        arrowPrefab, 
                        new Vector3(
                            arrowsStartingPoint.x + spaceBetweenEachArrow * (float)j, 
                            arrowsStartingPoint.y,
                            arrowsStartingPoint.z - spaceBetweenEachArrow * (float)i
                        ),
                        Quaternion.Euler(new Vector3(90f, 0f, 0f))
                        
                    )
                );
            }
         
        }

        buildSwipes();
        freezeTimerController.UnPauseTimer();
    }

    
    void Update()
    {
        if (gamePlayReadyStartAnimator.finished && !gameOver)
        {
            if (!gamePaused)
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
                        checkForWinCondition();
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
                                checkForWinCondition();
                            }
                        }
                    }
                }
            }
        }
        


        if (gameOver && !winLosePanelShown)
        {
            freezeTimerController.PauseTimer();

            if(gameWon)
            {
                // give the player their reward for winning
                // and show the win screne!

                // money was previously set from PlayerPrefs.GetInt("money")
                // lets add to it and save it
                int moneyEarned = difficultyLevel * 10;
                money += moneyEarned;
                PlayerPrefs.SetInt("money", money);

                gameplayMenus.setMoney(moneyEarned);
                gameplayMenus.showDrivewayCleared();
                
            }
            else
            {
                gameplayMenus.showYouFrozeToDeath();
            }
            winLosePanelShown = true;
        }
    }

    void clearRow()
    {
        //if (targetPoint > minScaleTile)
        float scaleFactor = (float)1 / (float)numberOfRowsOnThisLevel;
        float newSnowHeight = tileOriginalScale.z - (scaleFactor * tilePlayerIsOn);
        
        SnowMove.transform.localScale = new Vector3(tileOriginalScale.x, tileOriginalScale.y, newSnowHeight);
        
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
        makeAllArrowsBigAgain();
        numberOfPlayerSwipesForTheCurrentTile = 0;
        tilePlayerIsOn++;
        clearRow();
        buildSwipes();
        freezeTimerController.ResetTimer();
    }

    void checkForWinCondition()
    {
        // tilePlayerIsOn was incremented before this check
        if (tilePlayerIsOn >= numberOfRowsOnThisLevel)
        {
            gameWon = true;
            gameOver = true;
        }
    }

    void swipe(Directions passedInDirectionFromKeyboardControls = Directions.None)
    {
        // need to check if it is a vertical swipe or horizontal swipe first
        Vector2 distance = endPos - startPos;

        int swipeDirection = 0;

        // if you swiped
        if (passedInDirectionFromKeyboardControls == Directions.None)
        {
            //Debug.Log("SwipeControls");
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
            makeAllArrowsBigAgain();
            numberOfPlayerSwipesForTheCurrentTile = 0;
        }
        else
        {
            
            playerSwipesForThisTile[numberOfPlayerSwipesForTheCurrentTile] = swipeDirection;

            if (swipeDirection == (int)Directions.Up)
            {
                swipesToClearThisTileArrows[numberOfPlayerSwipesForTheCurrentTile].transform.localScale = new Vector3(.5f, .5f, .5f);
            }
            else if (swipeDirection == (int)Directions.Down)
            {
                swipesToClearThisTileArrows[numberOfPlayerSwipesForTheCurrentTile].transform.localScale = new Vector3(.5f, .5f, .5f);
            }
            else if (swipeDirection == (int)Directions.Left)
            {
                swipesToClearThisTileArrows[numberOfPlayerSwipesForTheCurrentTile].transform.localScale = new Vector3(.5f, .5f, .5f);
            }
            else if (swipeDirection == (int)Directions.Right)
            {

                swipesToClearThisTileArrows[numberOfPlayerSwipesForTheCurrentTile].transform.localScale = new Vector3(.5f, .5f, .5f);
            }
            numberOfPlayerSwipesForTheCurrentTile++;
        }
        
    }

    private void makeAllArrowsBigAgain()
    {
        for (int i = 0; i < swipesToClearThisTileArrows.Count; i++)
        {
            swipesToClearThisTileArrows[i].transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            swipesToClearThisTileArrows[i].GetComponent<Renderer>().material.color = Color.green;
        }
    }
    // Use this for initialization
    private void buildSwipes()
    {
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
                swipesToClearThisTileArrows[i].transform.rotation = Quaternion.Euler(new Vector3(90f, 0f, 90f));
            }
            else if (swipe == (int)Directions.Down)
            {
                swipesToClearThisTile[i] = (int)Directions.Down;
                swipesToClearThisTileArrows[i].transform.rotation = Quaternion.Euler(new Vector3(90f, 0f, 270f));
            }
            else if (swipe == (int)Directions.Left)
            {
                swipesToClearThisTile[i] = (int)Directions.Left;
                swipesToClearThisTileArrows[i].transform.rotation = Quaternion.Euler(new Vector3(90f, 0f, 180f));
            }
            else if (swipe == (int)Directions.Right)
            {
                swipesToClearThisTile[i] = (int)Directions.Right;
                swipesToClearThisTileArrows[i].transform.rotation = Quaternion.Euler(new Vector3(90f, 0f, 0f));
            }
            swipesToClearThisTileArrows[i].SetActive(true);
        }

        for (int i = numberOfSwipesNeededToCompleteCurrentTile; i < swipesToClearThisTileArrows.Count; i++)
        {
            swipesToClearThisTileArrows[i].SetActive(false);
        }
    }
}
