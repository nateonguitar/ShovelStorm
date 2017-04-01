using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CameraMovementNewGameScene : MonoBehaviour {

    // list.ElementAt(0)
    List<GameObject> cells = new List<GameObject>();
    Vector3 initialPosition;
    List<Vector3> cellCameraMovements = new List<Vector3>();
    List<float> cellDurations = new List<float>();
    float timeToTransitionBetweenCells = 5f;
    float timeSinceTransitionBetweenCellsStarted = 0;
    float timeSinceCurrentCellStarted;
    int currentCell = 0;
    bool readyToSwitchCells = true;
    bool transitioningDarkBetweenCells = false;
    bool transitioningLightBetweenCells = true;
    Image sceneTransitioner;

    public void Start()
    {
        // have to do this because 
        // timeSinceTransitionBetweenCellsStarted = timeToTransitionBetweenCells / 2;
        // produced 0.48 instead of 0.5 for some reason...

        timeSinceTransitionBetweenCellsStarted = (float)Mathf.Round(timeToTransitionBetweenCells*2) / 2;


        sceneTransitioner = GameObject.FindWithTag("SceneTransitioner").GetComponent<Image>(); ;
        cells.Add(GameObject.FindWithTag("Cell0"));
        cells.Add(GameObject.FindWithTag("Cell1"));
        cellDurations.Add(5f);
        cellDurations.Add(4f);
        cellCameraMovements.Add(new Vector3(-0.75f, 0.25f, -9.5f));
        cellCameraMovements.Add(new Vector3(-2f, 4f, -9.5f));
        initialPosition = transform.position;

        for(int i=1; i<cellDurations.Count; i++)
        {
            GameObject.FindWithTag("Cell" + i).SetActive(false);
        }
    }

    public void FixedUpdate()
    {
        // track this cell's progress through time
        timeSinceCurrentCellStarted += Time.deltaTime;

        // allow the cell to animate until it's time is up

        if(currentCell >= cells.Count)
        {
            SceneManager.LoadScene("LevelSelectMap");
        }
        else
        {
            if (timeSinceCurrentCellStarted >= cellDurations[currentCell] && !readyToSwitchCells)
            {
                Debug.Log("Check " + 1);
                readyToSwitchCells = true;
            }

            // TODO: I think we can combine /\ that if statement and \/ that if statement

            // if we are ready to darken the screen between cells
            if (readyToSwitchCells && !transitioningDarkBetweenCells && !transitioningLightBetweenCells)
            {
                Debug.Log("Check " + 2);
                // start the transition between cells: dark then light
                transitioningDarkBetweenCells = true;
                Debug.Log(timeSinceTransitionBetweenCellsStarted);
                Debug.Log(timeToTransitionBetweenCells / 2);
            }

            // transitioning darker between cells
            // timeSinceTransitionBetweenCellsStarted < timeToTransitionBetweenCells/2
            if (
                readyToSwitchCells
                && transitioningDarkBetweenCells
                && !transitioningLightBetweenCells
                && timeSinceTransitionBetweenCellsStarted <= timeToTransitionBetweenCells / 2)
            {


                Debug.Log("Check " + 3);
                float opacity = timeSinceTransitionBetweenCellsStarted / (timeToTransitionBetweenCells / 2);
                timeSinceTransitionBetweenCellsStarted += Time.deltaTime;
                Debug.Log(timeSinceTransitionBetweenCellsStarted);
                Debug.Log(timeToTransitionBetweenCells / 2);
                sceneTransitioner.color = new Color(
                    sceneTransitioner.color.r,
                    sceneTransitioner.color.g,
                    sceneTransitioner.color.b,
                    opacity
                );

                // test if we need to flip to transitioning lighter
                if (timeSinceTransitionBetweenCellsStarted >= timeToTransitionBetweenCells / 2)
                {
                    Debug.Log("Check " + 4);
                    // make sure the scene went completely black at the end of the darkening
                    sceneTransitioner.color = new Color(
                        sceneTransitioner.color.r,
                        sceneTransitioner.color.g,
                        sceneTransitioner.color.b,
                        1
                    );
                    transitioningDarkBetweenCells = false;
                    transitioningLightBetweenCells = true;

                    // switch to next cell
                    cells[currentCell].SetActive(false);
                    currentCell++;
                    if (currentCell < cells.Count)
                    {
                        cells[currentCell].SetActive(true);



                        // move camera back to original position
                        gameObject.transform.position = initialPosition;

                        // start shifting camera
                        StartCoroutine(MoveOverSeconds(gameObject, cellCameraMovements[currentCell], cellDurations[currentCell]));
                    }
                }
            }




            // transition lighter between cells
            if (
                readyToSwitchCells
                && !transitioningDarkBetweenCells
                && transitioningLightBetweenCells
                && timeSinceTransitionBetweenCellsStarted >= timeToTransitionBetweenCells / 2)
            {
                Debug.Log("Check " + 5);
                Debug.Log(timeSinceTransitionBetweenCellsStarted);
                timeSinceTransitionBetweenCellsStarted += Time.deltaTime;
                float opacity = 2 - timeSinceTransitionBetweenCellsStarted / (timeToTransitionBetweenCells / 2);

                sceneTransitioner.color = new Color(
                    sceneTransitioner.color.r,
                    sceneTransitioner.color.g,
                    sceneTransitioner.color.b,
                    opacity
                );

                // test if we need to stop transitioning lighter
                if (timeSinceTransitionBetweenCellsStarted >= timeToTransitionBetweenCells)
                {
                    Debug.Log("Check " + 6);
                    // make sure the scene went completely clear at the end of the lightening
                    sceneTransitioner.color = new Color(
                        sceneTransitioner.color.r,
                        sceneTransitioner.color.g,
                        sceneTransitioner.color.b,
                        0
                    );
                    transitioningDarkBetweenCells = false;
                    transitioningLightBetweenCells = false;

                    readyToSwitchCells = false;

                    // start the timer over for the next transition
                    timeSinceCurrentCellStarted = 0;
                    timeSinceTransitionBetweenCellsStarted = 0;

                    // start shifting camera
                    StartCoroutine(MoveOverSeconds(gameObject, cellCameraMovements[currentCell], cellDurations[currentCell]));
                }
            }
        }

        
    }

     
  public IEnumerator MoveOverSpeed(GameObject objectToMove, Vector3 end, float speed)
    {
        // speed should be 1 unit per second
        while (objectToMove.transform.position != end)
        {
            objectToMove.transform.position = Vector3.MoveTowards(objectToMove.transform.position, end, speed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
    }
    public IEnumerator MoveOverSeconds(GameObject objectToMove, Vector3 end, float seconds)
    {
        float elapsedTime = 0;
        Vector3 startingPos = objectToMove.transform.position;
        while (elapsedTime < seconds)
        {
            objectToMove.transform.position = Vector3.Lerp(startingPos, end, (elapsedTime / seconds));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        objectToMove.transform.position = end;
    }
}
