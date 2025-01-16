using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FoodScript : MonoBehaviour
{
    public GameObject snakeHead;
    CubeOrient cubeOrient;

    void Start()
    {
        RandomizeOrient();
        transform.position = cubeOrient.WorldPosition();
    }

    public void RandomizeOrient() {
        cubeOrient = new CubeOrient();
        do { cubeOrient.RandomizePosition(); }
        while (cubeOrient.WorldPosition() == snakeHead.transform.position ||
               snakeHead.GetComponent<SnakeHeadScript>().snakeBody.ToArray().Any(
                   x => x.transform.position == cubeOrient.WorldPosition()));
        transform.position = cubeOrient.WorldPosition();
    }

    void Update()
    {
        if (transform.position == snakeHead.transform.position) {
            RandomizeOrient();
            transform.position = cubeOrient.WorldPosition();
            snakeHead.GetComponent<SnakeHeadScript>().Grow();
        }
    }
}
