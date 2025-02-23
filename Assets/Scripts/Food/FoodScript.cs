using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public enum FoodBehavior
{
    Destroy,
    Reposition
}

public class FoodScript : MonoBehaviour
{
    public GameObject snakeHead;
    public CubeOrient cubeOrient;
    public Color color;
    public FoodBehavior foodBehavior;
    
    public int points, gold, duration = -1;
    public float growSpeed = 10f;

    SnakeManager snakeManager;
    GameObject foodManager;
    float currScale, timeLeft;
    bool canPickUp;

    public void Reset()
    {
        RandomizeOrient();
        timeLeft = duration + 1;
        canPickUp = true;
    }

    void Start()
    {
        snakeManager = snakeHead.GetComponent<SnakeManager>();
        foodManager = snakeManager.foodManager.gameObject;
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        MaterialPropertyBlock block = new MaterialPropertyBlock();
        meshRenderer.GetPropertyBlock(block);
        block.SetColor("_Color", color);
        meshRenderer.SetPropertyBlock(block);
        Reset();
    }

    public void RandomizeOrient()
    {
        cubeOrient = new CubeOrient();

        if (snakeManager == null) { snakeManager = snakeHead.GetComponent<SnakeManager>(); }

        do { cubeOrient.RandomizePosition(); }
        while (cubeOrient.WorldPosition() == snakeHead.transform.position ||
               snakeManager.snakeMove.snakeBody.ToArray().Any(
                   x => x.transform.position == cubeOrient.WorldPosition()) ||
               snakeManager.wallManager.transform.Cast<Transform>().Any(
                   x => x.transform.position == cubeOrient.WorldPosition())
        );
        transform.position = cubeOrient.WorldPosition();
        currScale = 0f;
    }

    public void Collect()
    {
        if (!canPickUp) { return; }
        FoodManager fm = foodManager.GetComponent<FoodManager>();
        snakeManager.snakeMove.Grow((int)Math.Ceiling(fm.foodMult * points + fm.foodAdd));
        snakeManager.goldManager.currGold += (int)Math.Ceiling(fm.goldMult * gold + fm.goldAdd);

        if (foodBehavior == FoodBehavior.Destroy) { Destroy(gameObject); }
        else if (foodBehavior == FoodBehavior.Reposition) { Reset(); }
    }

    public void OnTick()
    {
        timeLeft--;
        if (timeLeft == 0) { StartCoroutine(EndDuration()); }
    }

    void Update()
    {
        if (snakeManager == null) { snakeManager = snakeHead.GetComponent<SnakeManager>(); }
        
        if (transform.position == snakeHead.transform.position) { Collect(); }

        float desiredScale = Mathf.Sin(Time.time * 4) * 0.125f + 0.6875f;
        currScale = Mathf.Lerp(currScale, desiredScale, Time.deltaTime * growSpeed);
        transform.localScale = new Vector3(currScale, currScale, currScale);
    }

    IEnumerator EndDuration()
    {
        canPickUp = false;
        Vector3 startScale = transform.localScale;

        float scale = startScale.x;
        while (scale > 0.05f)
        {
            scale = Mathf.Lerp(scale, 0f, Time.deltaTime * growSpeed);
            transform.localScale = new Vector3(scale, scale, scale);
            yield return null;
        }

        if (foodBehavior == FoodBehavior.Destroy) { Destroy(gameObject); }
        else if (foodBehavior == FoodBehavior.Reposition) { Reset(); }
    }
}
