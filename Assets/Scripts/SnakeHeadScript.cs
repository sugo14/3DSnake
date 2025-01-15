using UnityEngine;

public class SnakeHeadScript : MonoBehaviour
{
    public float moveTime = 1f, debugScale = 1f;
    public Square currSquare = Square.Top;
    public CubeOrient orient;
    float timer;

    void Start()
    {
        timer = 0f;
        orient = new CubeOrient();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W)) { orient.UpInput(); }
        if (Input.GetKeyDown(KeyCode.S)) { orient.DownInput(); }
        if (Input.GetKeyDown(KeyCode.A)) { orient.LeftInput(); }
        if (Input.GetKeyDown(KeyCode.D)) { orient.RightInput(); }
        timer += Time.deltaTime;
        if (timer >= moveTime) {
            timer = 0;
            orient.Go();
            /* Debug.Log(orient.ToString()); */
            transform.position = orient.WorldPosition() * debugScale;
        }
    }
}
