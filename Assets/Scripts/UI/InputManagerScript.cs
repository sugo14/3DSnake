using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InputType {
    Up,
    Down,
    Left,
    Right,
    Tap,
    None
}

public class InputManager : MonoBehaviour
{
    private Vector3 fp;   //First touch position
    private Vector3 lp;   //Last touch position
    private float dragDistance;  //minimum distance for a swipe to be registered
    public SnakeManager snakeManager;
    public InputType currInput;
    bool inputSet = false;
    public Joystick joystick;

    void Start()
    {
        dragDistance = Screen.height * 15 / 100; //dragDistance is 15% height of the screen
    }

    InputType DetectTouch()
    {
        if (Input.touchCount == 1) // user is touching the screen with a single touch
        {
            Touch touch = Input.GetTouch(0); // get the touch
            if (touch.phase == TouchPhase.Began) //check for the first touch
            {
                fp = touch.position;
                lp = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved) // update the last position based on where they moved
            {
                lp = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended) //check if the finger is removed from the screen
            {
                lp = touch.position;  //last touch position. Ommitted if you use list

                /* //Check if drag distance is greater than 20% of the screen height
                if (Mathf.Abs(lp.x - fp.x) > dragDistance || Mathf.Abs(lp.y - fp.y) > dragDistance)
                {
                    if (Mathf.Abs(lp.x - fp.x) > Mathf.Abs(lp.y - fp.y))
                    {   //If the horizontal movement is greater than the vertical movement...
                        if ((lp.x > fp.x)) { return InputType.Right; }
                        else { return InputType.Left; }
                    }
                    else
                    {   //the vertical movement is greater than the horizontal movement
                        if (lp.y > fp.y) { return InputType.Up; }
                        else { return InputType.Down; }
                    }
                }W
                else { */ return InputType.Tap;/*  } */
            }
        }
        return InputType.None;
    }

    void Update()
    {
        if (!inputSet) { currInput = DetectTouch(); }
        /* if (currInput == InputType.Up) { snakeManager.snakeMove.orient.UpInput(); }
        else if (currInput == InputType.Down) { snakeManager.snakeMove.orient.DownInput(); }
        else if (currInput == InputType.Left) { snakeManager.snakeMove.orient.LeftInput(); }
        else if (currInput == InputType.Right) { snakeManager.snakeMove.orient.RightInput(); } */
        // get the largest value of the joystick
        if (Math.Abs(joystick.Horizontal) > Math.Abs(joystick.Vertical)) {
            if (joystick.Horizontal > 0.3f) { snakeManager.snakeMove.orient.RightInput(); }
            if (joystick.Horizontal < -0.3f) { snakeManager.snakeMove.orient.LeftInput(); }
        }
        else {
            if (joystick.Vertical > 0.3f) { snakeManager.snakeMove.orient.UpInput(); }
            if (joystick.Vertical < -0.3f) { snakeManager.snakeMove.orient.DownInput(); }
        }
        inputSet = false;
    }

    public void UpInput() {
        currInput = InputType.Up;
        inputSet = true;
    }
    public void DownInput() {
        currInput = InputType.Down;
        inputSet = true;
    }
    public void LeftInput() {
        currInput = InputType.Left;
        inputSet = true;
    }
    public void RightInput() {
        currInput = InputType.Right;
        inputSet = true;
    }
}
