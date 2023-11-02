using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileImput : MonoBehaviour
{
    private const float DEATHZONE = 100.0f;
    public static MobileImput Instance { set; get; }
    private bool tap,swipeLeft, swipeRight,swipeUp,swipeDown;
    private Vector2 swipeDelta, startTouch;
    public bool Tap { get { return tap; } }
    public Vector2 SwipeDelta { get { return swipeDelta; } }
    public bool SwipeLeft { get { return swipeLeft; } }
    public bool SwipeRight { get { return swipeRight; } }
    public bool SwipeUp { get { return swipeUp; } }
    public bool SwipeDown { get { return swipeDown; } }

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        //Reseting all the booleans
        tap = swipeLeft = swipeRight =  swipeDown = swipeUp = false;

        // let's check for inputs
        #region Standalone Inputs
        if(Input.GetMouseButtonDown(0))
        {
            tap = true;
            startTouch = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            startTouch = swipeDelta = Vector2.zero;
        }
        #endregion

        #region Mobile Inputs
        if (Input.touches.Length != 0)
        {
            if (Input.touches[0].phase == TouchPhase.Began)
            {
                tap = true;
                startTouch = Input.mousePosition;
            }
            else if (Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled)
            {
                startTouch = swipeDelta = Vector2.zero;
            }

        }
       
        #endregion


        //  Calculate distance
        swipeDelta = Vector2.zero;

        if(startTouch != Vector2.zero)
        {
            // Let's check with mobile
            if (Input.touches.Length != 0)
            {
                swipeDelta = Input.touches[0].position - startTouch;
            }
            // Let's check with standalone
            else if(Input.GetMouseButton(0))
            {
                swipeDelta = (Vector2)Input.mousePosition - startTouch;
            }
        }

        //  Let's Check  if we're beyond the deadzone
        if(swipeDelta.magnitude > DEATHZONE)
        {
            //this is aconfirmed
            float x = swipeDelta.x;
            float y = swipeDelta.y;

            if(Mathf.Abs(x) > Mathf.Abs(y))
            {
                // Left or Right
                if(x < 0)
                {
                    swipeLeft = true;
                }
                else
                {
                    swipeRight = true;
                }

            }
            else
            {
                //Up or down
                if (y < 0)
                {
                    swipeDown = true;
                }
                else
                {
                    swipeUp = true;
                }
            }

            startTouch = swipeDelta = Vector2.zero;
        }


    }
}
