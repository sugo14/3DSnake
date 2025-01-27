using System;
using System.Collections.Generic;
using UnityEngine;

public class SnakeMove : MonoBehaviour
{
    public SnakeManager manager;

    public KeyCode upKey = KeyCode.W;
    public KeyCode downKey = KeyCode.S;
    public KeyCode leftKey = KeyCode.A;
    public KeyCode rightKey = KeyCode.D;

    public GameObject snakeBodyPrefab;

    public int currLength;
    public int lengthMod;

    // the most recent body is at the front of the list
    public List<GameObject> snakeBody = new List<GameObject>();
    public CubeOrient orient;

    Vector2Int wantDir;
    int idx;

    public void Reset()
    {
        foreach (GameObject body in snakeBody) { Destroy(body); }
        snakeBody.Clear();
        currLength = 1;
        orient = new CubeOrient();
        transform.position = orient.WorldPosition();
        wantDir = Vector2Int.zero;
        idx = 0;
        GetComponent<MeshRenderer>().material = manager.snakeSpecies.snakeSpecies.headMaterial;
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

        GameObject newBody = Instantiate(snakeBodyPrefab, transform.position, Quaternion.identity);
        newBody.GetComponent<MeshRenderer>().material = manager.snakeSpecies.snakeSpecies.bodyMaterials[idx++ % manager.snakeSpecies.snakeSpecies.bodyMaterials.Count];
        newBody.GetComponent<SnakeBody>().cubeOrient = CubeOrient.Copy(orient);
        snakeBody.Insert(0, newBody);
        while (snakeBody.Count > Math.Max(currLength + lengthMod, 0))
        {
            GameObject lastBody = snakeBody[snakeBody.Count - 1];
            snakeBody.RemoveAt(snakeBody.Count - 1);
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
