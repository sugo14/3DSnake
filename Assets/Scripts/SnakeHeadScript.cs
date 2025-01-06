using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeHeadScript : MonoBehaviour
{
    public Vector3 dir;
    public float moveTime;

    bool wantsUp, wantsDown, wantsLeft, wantsRight;
    float timer;

    // Start is called before the first frame update
    void Start()
    {
        dir = new Vector3(1, 0, 0);
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
        /* timer += Time.deltaTime;
        if (timer >= moveTime) { */
            Vector3 newDir = Vector3.zero;
            if (wantsUp == true) {
                newDir = new Vector3(-dir.y, dir.x, dir.z);
                dir = newDir;
            }
            wantsUp = false;
            wantsDown = false;
            wantsLeft = false;
            wantsRight = false;
        /* } */
        transform.position += dir * Time.deltaTime;
    }
}
