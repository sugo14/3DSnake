using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeHeadScript : MonoBehaviour
{
    public float moveTime = 1f, debugScale = 1f;
    public CubeOrient orient;
    public int growLength = 3;

    public GameObject snakeBodyPrefab;
    Vector2Int wantDir;

    float timer;
    public int currLength;
    public Queue<GameObject> snakeBody = new Queue<GameObject>();

    public void Grow() { currLength += growLength; }

    public void Reset() {
        foreach (GameObject body in snakeBody) { Destroy(body); }
        snakeBody.Clear();
        currLength = 1;
        orient = new CubeOrient();
        transform.position = orient.WorldPosition() * debugScale;
    }

    public void Hide() {
        foreach (GameObject body in snakeBody) { body.SetActive(false); }
        gameObject.SetActive(false);
    }
    public void Show() {
        foreach (GameObject body in snakeBody) { body.SetActive(true); }
        gameObject.SetActive(true);
    }

    void Start()
    {
        timer = 0f;
        currLength = 1;
        orient = new CubeOrient();
        wantDir = Vector2Int.zero;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A) && !Input.GetKeyDown(KeyCode.D)) { wantDir = Vector2Int.left; }
        if (Input.GetKeyDown(KeyCode.D) && !Input.GetKeyDown(KeyCode.A)) { wantDir = Vector2Int.right; }
        if (Input.GetKeyDown(KeyCode.W) && !Input.GetKeyDown(KeyCode.S)) { wantDir = Vector2Int.up; }
        if (Input.GetKeyDown(KeyCode.S) && !Input.GetKeyDown(KeyCode.W)) { wantDir = Vector2Int.down; }
        timer += Time.deltaTime;
        if (timer >= moveTime) {
            if (wantDir == Vector2Int.up) { orient.UpInput(); }
            if (wantDir == Vector2Int.down) { orient.DownInput(); }
            if (wantDir == Vector2Int.left) { orient.LeftInput(); }
            if (wantDir == Vector2Int.right) { orient.RightInput(); }
            wantDir = Vector2Int.zero;
            snakeBody.Enqueue(Instantiate(snakeBodyPrefab, transform.position, Quaternion.identity));
            if (snakeBody.Count > currLength) {
                GameObject lastBody = snakeBody.Dequeue();
                Destroy(lastBody);
            }
            timer = 0;
            orient.Go();
            /* Debug.Log(orient.ToString()); */
            transform.position = orient.WorldPosition() * debugScale;
        }
    }
}
