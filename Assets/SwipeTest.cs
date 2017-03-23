using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeTest : MonoBehaviour
{
    public float maxTime = 0.5f;
    public float minSwipeDist = 50f;

    float startTime;
    float endTime;

    Vector3 startPos;
    Vector3 endPos;

    float swipeDistance;
    float swipeTime;


    void Update()
    {
        if(Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if(touch.phase == TouchPhase.Began)
            {
                startTime = Time.time;
                startPos = touch.position;
            }
            else if(touch.phase == TouchPhase.Ended)
            {
                endTime = Time.time;
                endPos = touch.position;

                swipeDistance = (endPos - startPos).magnitude;
                swipeTime = endTime - startTime;

                if(swipeTime < maxTime && swipeDistance > minSwipeDist)
                {
                    swipe();
                }
            }
        }
    }

    void swipe()
    {
        // need to check if it is a vertical swipe or horizontal swipe first
        Vector2 distance = endPos - startPos;

        if(Mathf.Abs(distance.x) > Mathf.Abs(distance.y)){
            //Debug.Log("Horizontal Swipe");
            if(distance.x > 0)
            {
                Debug.Log("Right Swipe");
            }
            else if(distance.x < 0)
            {
                Debug.Log("Left Swipe");
            }
        }
        else if(Mathf.Abs(distance.x) < Mathf.Abs(distance.y)){
            //Debug.Log("Vertical Swipe");
            if(distance.y > 0)
            {
                Debug.Log("Up Swipe");
            }
            else if(distance.y < 0)
            {
                Debug.Log("Down Swipe");
            }
        }
    }
}
