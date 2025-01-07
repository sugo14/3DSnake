using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public enum Square {
    Top, Bottom, Left, Right, Front, Back
};

public class SnakeHeadScript : MonoBehaviour
{
    public Vector2 dir;
    public float moveTime = 1f;
    public Square currSquare = Square.Top;

    bool wantsUp, wantsDown, wantsLeft, wantsRight;
    float timer;

    // Start is called before the first frame update
    void Start()
    {
        dir = new Vector2(1, 0);
        wantsUp = false; wantsDown = false; wantsLeft = false; wantsRight = false;
        timer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W)) {
            wantsUp = true;
        }
        if (Input.GetKeyDown(KeyCode.S)) {
            wantsDown = true;
        }
        if (Input.GetKeyDown(KeyCode.A)) {
            wantsLeft = true;
        }
        if (Input.GetKeyDown(KeyCode.D)) {
            wantsRight = true;
        }
        timer += Time.deltaTime;
        if (timer >= moveTime) {
            Vector3 newDir = Vector3.zero;
            if (wantsUp == true) {
                newDir = new Vector2(1, 0);
                dir = newDir;
            }
            wantsUp = false;
            wantsDown = false;
            wantsLeft = false;
            wantsRight = false;
            timer = 0;
            transform.position += new Vector3(newDir.x, newDir.y, 0);
        }
    }
}
