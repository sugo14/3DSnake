using UnityEngine;

public class SnakeManager : MonoBehaviour
{
    public SnakeMove snakeMove;
    public Abilities abilities;

    public float tickTime = 0.3f;

    float timer;

    public void Reset()
    {
        snakeMove.Reset();
        abilities.Reset();
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
}
