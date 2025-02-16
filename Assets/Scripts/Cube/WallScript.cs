using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallScript : MonoBehaviour
{
    public GameObject wallPrefab;
    public int wallCount = 10, maxWidth = 3, maxHeight = 3;

    void InitializeWall(int x, int y, CubeOrient cubeOrient)
    {
        for (int i = 0; i < x; i++)
        {
            CubeOrient co = CubeOrient.Copy(cubeOrient);
            co.goingInWorldUp = false;
            co.dir = Vector2Int.right;
            for (int j = 0; j < i; j++)
            {
                co.Go();
                co.goingInWorldUp = false;
            }
            co.dir = CubeOrient.Left(co.dir);
            for (int j = 0; j < y; j++)
            {
                GameObject wall = Instantiate(wallPrefab, co.WorldPosition(), Quaternion.identity);
                wall.transform.parent = transform;
                co.Go();
                co.goingInWorldUp = false;
            }
        }
    }

    public void InitializeAllWalls()
    {
        CubeOrient cubeOrient = new CubeOrient();
        for (int i = 0; i < wallCount; i++) {
            cubeOrient.RandomizePosition();
            int w = Random.Range(1, maxWidth), h = Random.Range(1, maxHeight);
            InitializeWall(w, h, cubeOrient);
        }
    }

    public void ClearWalls()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
}
