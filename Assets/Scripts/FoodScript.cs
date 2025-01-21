using System.Linq;
using UnityEngine;

public class FoodScript : MonoBehaviour
{
    public GameObject snakeHead;
    public int points;

    SnakeManager snakeManager;
    CubeOrient cubeOrient;

    void Start()
    {
        snakeManager = snakeHead.GetComponent<SnakeManager>();
        RandomizeOrient();
        transform.position = cubeOrient.WorldPosition();
    }

    public void RandomizeOrient() {
        cubeOrient = new CubeOrient();
        do { cubeOrient.RandomizePosition(); }
        while (cubeOrient.WorldPosition() == snakeHead.transform.position ||
               snakeManager.snakeMove.snakeBody.ToArray().Any(
                   x => x.transform.position == cubeOrient.WorldPosition()));
        transform.position = cubeOrient.WorldPosition();
    }

    void Update()
    {
        if (snakeManager == null) { snakeManager = snakeHead.GetComponent<SnakeManager>(); }
        
        if (transform.position == snakeHead.transform.position) {
            RandomizeOrient();
            transform.position = cubeOrient.WorldPosition();
            snakeManager.snakeMove.Grow(points);
        }
    }
}
