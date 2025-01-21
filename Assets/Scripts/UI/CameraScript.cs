using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public GameObject snakeHead, cubePrefab;
    public int minDist = 25, maxDist = 70;
    public int cubeCount = 300, minSize = 1, maxSize = 9;
    public float distance = 10, rotationSpeed = 2, moveSpeed = 2, foresight = 2;

    SnakeManager snakeManager;

    void Start()
    {
        snakeManager = snakeHead.GetComponent<SnakeManager>();
        for (int i = 0; i < cubeCount; i++)
        {
            GameObject instance = Instantiate(cubePrefab);
            int x = Random.Range(0, 3);
            instance.transform.position = new Vector3
            (
                x == 0 ? ((Random.Range(0, 2) == 1 ? 1 : -1) * Random.Range(minDist, maxDist + 1)) : Random.Range(-maxDist, maxDist + 1),
                x == 1 ? ((Random.Range(0, 2) == 1 ? 1 : -1) * Random.Range(minDist, maxDist + 1)) : Random.Range(-maxDist, maxDist + 1),
                x == 2 ? ((Random.Range(0, 2) == 1 ? 1 : -1) * Random.Range(minDist, maxDist + 1)) : Random.Range(-maxDist, maxDist + 1)
            );
            int scale = Random.Range(minSize, maxSize);
            instance.transform.localScale = new Vector3(scale, scale, scale);
        }
    }

    void Update()
    {
        if (snakeManager == null) { snakeManager = snakeHead.GetComponent<SnakeManager>(); }

        Transform t = gameObject.transform;
        CubeOrient cubeOrient = snakeManager.snakeMove.orient;

        cubeOrient = CubeOrient.Copy(cubeOrient);
        for (int i = 0; i < foresight; i++) { cubeOrient.Go(); }

        Vector3 futurePos = cubeOrient.WorldPosition();
        Vector3 desiredPos = futurePos + Vector3.ClampMagnitude(futurePos, 1) * distance;
        Quaternion desiredRot = Quaternion.LookRotation(Vector3.zero - desiredPos, snakeManager.snakeMove.orient.SnakeUp());

        transform.position = Vector3.Slerp(transform.position, desiredPos, moveSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, desiredRot, rotationSpeed * Time.deltaTime);
    }
}
