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
    public GameObject wallManager;

    public float tickTime = 0.3f;
    public int isInvincibleCnt = 0;
    public float timer;

    public void Reset()
    {
        snakeMove.Reset();
        abilities.Reset();
        effectManager.Reset();
        permEffectManager.Reset();
        isInvincibleCnt = 0;
        shopScript.Reset();
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
    }

    void Update()
    {
        timer += Time.deltaTime;
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
            Debug.Log(wall.transform.position);
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
