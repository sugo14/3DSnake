using System;
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
            int x = UnityEngine.Random.Range(0, 3);
            instance.transform.position = new Vector3
            ( // this is not a good random distribution
                x == 0 ? ((UnityEngine.Random.Range(0, 2) == 1 ? 1 : -1) * UnityEngine.Random.Range(minDist, maxDist + 1)) : UnityEngine.Random.Range(-maxDist, maxDist + 1),
                x == 1 ? ((UnityEngine.Random.Range(0, 2) == 1 ? 1 : -1) * UnityEngine.Random.Range(minDist, maxDist + 1)) : UnityEngine.Random.Range(-maxDist, maxDist + 1),
                x == 2 ? ((UnityEngine.Random.Range(0, 2) == 1 ? 1 : -1) * UnityEngine.Random.Range(minDist, maxDist + 1)) : UnityEngine.Random.Range(-maxDist, maxDist + 1)
            );
            int scale = UnityEngine.Random.Range(minSize, maxSize);
            instance.transform.localScale = new Vector3(scale, scale, scale);
        }
    }

    void Update()
    {
        if (snakeManager == null) { snakeManager = snakeHead.GetComponent<SnakeManager>(); }

        CubeOrient cubeOrient = snakeManager.snakeMove.orient;

        cubeOrient = CubeOrient.Copy(cubeOrient);
        for (int i = 0; i < foresight; i++) { cubeOrient.Go(); }
        Vector3 delta = cubeOrient.WorldPosition() - snakeManager.snakeMove.orient.WorldPosition();
        delta = new Vector3((float)(Math.Sign(delta.x) * Math.Sqrt(Math.Abs(delta.x * 10)) / 10), (float)(Math.Sign(delta.y) * Math.Sqrt(Math.Abs(delta.y * 10)) / 10), (float)(Math.Sign(delta.z) * Math.Sqrt(Math.Abs(delta.z * 10)) / 10));
        Vector3 futurePos = cubeOrient.WorldPosition() + delta;

        //Vector3 futurePos = cubeOrient.WorldPosition();
        Vector3 desiredPos = futurePos + Vector3.ClampMagnitude(futurePos, 1) * distance;
        Quaternion desiredRot = Quaternion.LookRotation(Vector3.zero - desiredPos, cubeOrient.SnakeUp());

        transform.position = Vector3.Slerp(transform.position, desiredPos, moveSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, desiredRot, rotationSpeed * Time.deltaTime);
    }
}
