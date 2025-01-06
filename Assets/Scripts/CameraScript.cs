using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public GameObject snakeHead;
    public float distance = 10, camSpeed = 20;
    public bool firstPerson = false;
    Vector3 currPos;

    // Start is called before the first frame update
    void Start()
    {
        currPos = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (firstPerson) {
            Vector2 targetPos = snakeHead.transform.position - snakeHead.GetComponent<SnakeHeadScript>().dir * -0.01f;
            gameObject.transform.position = targetPos;
        }
        else {
            Vector2 targetPos = snakeHead.GetComponent<SnakeHeadScript>().dir;
            currPos = Vector3.Slerp(currPos, targetPos, 0.001f * camSpeed);
            gameObject.transform.position = snakeHead.transform.position + currPos * distance;
        }
        gameObject.transform.rotation = Quaternion.LookRotation(snakeHead.transform.position - gameObject.transform.position, Vector3.down);
    }
}
