using System.Collections.Generic;
using UnityEngine;

public class SnakeMove : MonoBehaviour
{
    public KeyCode upKey = KeyCode.W;
    public KeyCode downKey = KeyCode.S;
    public KeyCode leftKey = KeyCode.A;
    public KeyCode rightKey = KeyCode.D;

    public GameObject snakeBodyPrefab;

    public bool isInvincible;
    public int currLength;

    public Queue<GameObject> snakeBody = new Queue<GameObject>();
    public CubeOrient orient;

    Vector2Int wantDir;

    public void Reset()
    {
        foreach (GameObject body in snakeBody) { Destroy(body); }
        snakeBody.Clear();
        currLength = 1;
        orient = new CubeOrient();
        transform.position = orient.WorldPosition();
        wantDir = Vector2Int.zero;
        isInvincible = false;
    }

    void Start() { Reset(); }

    public void OnTick()
    {
        if (wantDir == Vector2Int.up) { orient.UpInput(); }
        if (wantDir == Vector2Int.down) { orient.DownInput(); }
        if (wantDir == Vector2Int.left) { orient.LeftInput(); }
        if (wantDir == Vector2Int.right) { orient.RightInput(); }

        if (wantDir != Vector2Int.zero) { Debug.Log(orient.ToString()); } // DEBUG

        wantDir = Vector2Int.zero;

        snakeBody.Enqueue(Instantiate(snakeBodyPrefab, transform.position, Quaternion.identity));
        while (snakeBody.Count > currLength)
        {
            GameObject lastBody = snakeBody.Dequeue();
            Destroy(lastBody);
        }
        orient.Go();
        transform.position = orient.WorldPosition();
    }

    void Update()
    {
        if (Input.GetKeyDown(leftKey) && !Input.GetKeyDown(rightKey)) { wantDir = Vector2Int.left; }
        if (Input.GetKeyDown(rightKey) && !Input.GetKeyDown(leftKey)) { wantDir = Vector2Int.right; }
        if (Input.GetKeyDown(upKey) && !Input.GetKeyDown(downKey)) { wantDir = Vector2Int.up; }
        if (Input.GetKeyDown(downKey) && !Input.GetKeyDown(upKey)) { wantDir = Vector2Int.down; }
    }

    public void Grow(int points) { currLength += points; }

    public void Hide()
    {
        foreach (GameObject body in snakeBody) { body.SetActive(false); }
        gameObject.SetActive(false);
    }
    public void Show()
    {
        foreach (GameObject body in snakeBody) { body.SetActive(true); }
        gameObject.SetActive(true);
    }
}
