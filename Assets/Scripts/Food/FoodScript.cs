using System;
using System.Linq;
using UnityEngine;

public class FoodScript : MonoBehaviour
{
    public GameObject snakeHead;
    public GameObject foodManager;
    public CubeOrient cubeOrient;
    public Color color;
    
    public int points;
    public float growSpeed = 10f;

    SnakeManager snakeManager;
    float currScale;

    void Start()
    {
        snakeManager = snakeHead.GetComponent<SnakeManager>();
        RandomizeOrient();
        transform.position = cubeOrient.WorldPosition();
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        MaterialPropertyBlock block = new MaterialPropertyBlock();
        meshRenderer.GetPropertyBlock(block);
        block.SetColor("_Color", color);
        meshRenderer.SetPropertyBlock(block);
    }

    public void RandomizeOrient()
    {
        cubeOrient = new CubeOrient();
        do { cubeOrient.RandomizePosition(); }
        while (cubeOrient.WorldPosition() == snakeHead.transform.position ||
               snakeManager.snakeMove.snakeBody.ToArray().Any(
                   x => x.transform.position == cubeOrient.WorldPosition()));
        transform.position = cubeOrient.WorldPosition();
        currScale = 0f;
    }

    public void Collect()
    {
        RandomizeOrient();
        transform.position = cubeOrient.WorldPosition();
        FoodManager fm = foodManager.GetComponent<FoodManager>();
        snakeManager.snakeMove.Grow((int)Math.Ceiling(fm.foodMult * points + fm.foodAdd));
        currScale = 0f;
    }

    void Update()
    {
        if (snakeManager == null) { snakeManager = snakeHead.GetComponent<SnakeManager>(); }
        
        if (transform.position == snakeHead.transform.position) { Collect(); }
        float desiredScale = Mathf.Sin(Time.time * 4) * 0.125f + 0.6875f;
        currScale = Mathf.Lerp(currScale, desiredScale, Time.deltaTime * growSpeed);
        transform.localScale = new Vector3(currScale, currScale, currScale);
    }
}
