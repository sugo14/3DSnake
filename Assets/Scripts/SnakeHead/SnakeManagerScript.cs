using UnityEngine;
using System.Collections.Generic;
using System.Linq;


public class SnakeManager : MonoBehaviour
{
    public SnakeMove snakeMove;
    public Abilities abilities;
    public EffectManager effectManager;
    public SnakeSpecies snakeSpecies;
    public PermEffectManager permEffectManager;
    public FoodManager foodManager;
    public ShopScript shopScript;
    public WallScript wallManager;
    public GoldManager goldManager;

    public float tickTime = 0.3f;
    public int isInvincibleCnt = 0;
    public bool paused = false;
    public float timer;

    public void Reset()
    {
        snakeMove.Reset();
        abilities.Reset();
        effectManager.Reset();
        permEffectManager.Reset();
        isInvincibleCnt = 0;
        shopScript.Reset();
        foodManager.Reset();
        goldManager.Reset();
    }

    void Start()
    {
        timer = 0f;
    }

    void OnTick()
    {
        abilities.OnTick();
        snakeMove.OnTick();
        effectManager.OnTick();
        foreach (Transform food in foodManager.transform)
        {
            FoodScript foodScript = food.GetComponent<FoodScript>();
            foodScript.OnTick();
        }
    }

    void Update()
    {
        if (!paused) { timer += Time.deltaTime; }
        if (timer >= tickTime)
        {
            OnTick();
            timer = 0f;
        }
    }

    public bool IsDead()
    {
        foreach (Transform wall in wallManager.transform)
        {
            if (wall.transform.position == transform.position)
            {
                return true;
            }
        }
        return effectManager.isInvincible == 0 && 
               snakeMove.snakeBody.ToArray().Any
               (
                    x => x.transform.position == transform.position
               );
    }
}
