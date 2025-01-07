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

    /* // Update is called once per frame
    void LateUpdate()
    {
        Vector2 targetPos = snakeHead.GetComponent<SnakeHeadScript>().dir;
        currPos = Vector3.Slerp(currPos, targetPos, 0.3f * Time.deltaTime * camSpeed);
        gameObject.transform.position = snakeHead.transform.position + currPos * distance;
        Transform t = gameObject.transform;
        t.rotation = Quaternion.LookRotation(snakeHead.transform.position - t.position, Vector3.up);
    } */

    void Update() {
        Transform t = gameObject.transform;
        t.position = snakeHead.transform.position + Vector3.ClampMagnitude(snakeHead.transform.position, 1) * 10;
        t.rotation = Quaternion.LookRotation(Vector3.zero - t.position, Vector3.up);
    }
}
