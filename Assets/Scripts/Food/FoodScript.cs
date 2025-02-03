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

    SnakeManager snakeManager;

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
    }

    public void Collect()
    {
        RandomizeOrient();
        transform.position = cubeOrient.WorldPosition();
        FoodManager fm = foodManager.GetComponent<FoodManager>();
        snakeManager.snakeMove.Grow((int)Math.Ceiling(fm.foodMult * points + fm.foodAdd));
    }

    void Update()
    {
        if (snakeManager == null) { snakeManager = snakeHead.GetComponent<SnakeManager>(); }
        
        if (transform.position == snakeHead.transform.position) { Collect(); }
        float scale = Mathf.Sin(Time.time * 4) * 0.1f + 0.7f;
        transform.localScale = new Vector3(scale, scale, scale);
    }
}
