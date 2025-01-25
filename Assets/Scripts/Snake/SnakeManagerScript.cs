using UnityEngine;
using System.Collections.Generic;
using System.Linq;


public class SnakeManager : MonoBehaviour
{
    public SnakeMove snakeMove;
    public Abilities abilities;
    public EffectManager effectManager;

    public float tickTime = 0.3f;
    public int isInvincibleCnt = 0;

    float timer;

    public void Reset()
    {
        snakeMove.Reset();
        abilities.Reset();
        isInvincibleCnt = 0;
    }

    void Start()
    {
        timer = 0f;
    }

    void OnTick()
    {
        Debug.Log("Tick");
        snakeMove.OnTick();
        abilities.OnTick();
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
        return effectManager.isInvincible == 0 && 
               snakeMove.snakeBody.ToArray().Any
               (
                    x => x.transform.position == transform.position
               );
    }
}
