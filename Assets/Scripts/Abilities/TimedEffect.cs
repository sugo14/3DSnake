using System;
using UnityEngine;

public class TimedEffect : Effect
{
    public virtual int turns { get; set; }

    public TimedEffect(int turns) { this.turns = turns; }

    public override void OnTick(SnakeManager snakeManager) { turns--; }
    public override bool WantsToDetach() { return turns <= 0; }
}

public class None : TimedEffect
{
    public None() : base(0) { }
}

public class SpeedChange : TimedEffect
{
    public float speedMultiplier;

    public SpeedChange(int turns, float speedMultiplier) : base(turns)
    {
        this.speedMultiplier = speedMultiplier;
    }

    public override void Apply(SnakeManager snakeManager) { snakeManager.tickTime /= speedMultiplier; }
    public override void Remove(SnakeManager snakeManager) { snakeManager.tickTime *= speedMultiplier; }
}

public class Invincibility : TimedEffect
{
    public Invincibility(int turns) : base(turns) { }

    public override void Apply(SnakeManager snakeManager) { snakeManager.effectManager.isInvincible++; }
    public override void Remove(SnakeManager snakeManager) { snakeManager.effectManager.isInvincible--; }
}

public class ReduceLength : TimedEffect
{
    public int length;

    public ReduceLength(int length) : base(1)
    {
        this.length = length;
    }

    public override void Apply(SnakeManager snakeManager) { snakeManager.snakeMove.lengthMod -= length; }
    public override void Remove(SnakeManager snakeManager) { snakeManager.snakeMove.lengthMod += length; }
}

public class MoveForward : TimedEffect
{
    public int dist;

    public MoveForward(int dist) : base(1)
    {
        this.dist = dist;
    }

    public override void Apply(SnakeManager snakeManager)
    {
        CubeOrient orient = snakeManager.snakeMove.orient;
        for (int i = 0; i < dist; i++)
        {
            orient.Go();
        }
        snakeManager.snakeMove.orient = orient;
    }
}

public class RemoveScore : TimedEffect
{
    public int score;

    public RemoveScore(int score) : base(1)
    {
        this.score = score;
    }

    public override void Apply(SnakeManager snakeManager) { snakeManager.snakeMove.currLength = Math.Max(snakeManager.snakeMove.currLength - score, 0); }
}

public class CollectFoodInLine : TimedEffect
{
    public CollectFoodInLine() : base(1) { }

    public override void Apply(SnakeManager snakeManager)
    {
        CubeOrient orient = CubeOrient.Copy(snakeManager.snakeMove.orient);
        CubeOrient originalOrient = CubeOrient.Copy(orient);
        do
        {
            orient.Go();
            foreach (Transform child in snakeManager.foodManager.transform)
            {
                if (child.position == orient.WorldPosition())
                {
                    FoodScript foodScript = child.GetComponent<FoodScript>();
                    foodScript.Collect();
                }
            }
        } while (!(orient.WorldPosition() == originalOrient.WorldPosition()));
    }
}

public class CollectFoodOnFace : TimedEffect
{
    public CollectFoodOnFace() : base(1) { }

    public override void Apply(SnakeManager snakeManager)
    {
        foreach (Transform child in snakeManager.foodManager.transform)
        {
            if (child.GetComponent<FoodScript>().cubeOrient.square == snakeManager.snakeMove.orient.square)
            {
                FoodScript foodScript = child.GetComponent<FoodScript>();
                foodScript.Collect();
            }
        }
    }
}

public class GainPoints : TimedEffect
{
    public int points;

    public GainPoints(int points) : base(1)
    {
        this.points = points;
    }

    public override void Apply(SnakeManager snakeManager) { snakeManager.snakeMove.currLength += points; }
}

public class Reverse : TimedEffect
{
    public int depth;

    public Reverse(int depth) : base(1)
    {
        this.depth = depth;
    }

    public override void Apply(SnakeManager snakeManager)
    {
        if (snakeManager.snakeMove.snakeBody.Count == 0) { return; }
        for (int i = 0; i < depth; i++)
        {
            if (snakeManager.snakeMove.snakeBody.Count == 1) { break; }
            GameObject lastBody = snakeManager.snakeMove.snakeBody[0];
            snakeManager.snakeMove.snakeBody.RemoveAt(0);
            GameObject.Destroy(lastBody);
            GameObject lastCube = snakeManager.snakeMove.intermediaryCubes[0];
            snakeManager.snakeMove.intermediaryCubes.RemoveAt(0);
            GameObject.Destroy(lastCube);
        }
        GameObject mostRecent = snakeManager.snakeMove.snakeBody[0];
        snakeManager.snakeMove.orient = CubeOrient.Copy(mostRecent.GetComponent<SnakeBody>().cubeOrient);
        snakeManager.transform.position = snakeManager.snakeMove.orient.WorldPosition();
    }
}

public class SpawnFood : TimedEffect
{
    public int durAdd, durMult, points, gold;
    public Color foodColor;

    GameObject foodInstance;

    public SpawnFood(int durAdd, int durMult, int points, int gold, Color foodColor) : base(0)
    {
        this.durAdd = durAdd;
        this.durMult = durMult;
        this.points = points;
        this.gold = gold;
        this.foodColor = foodColor;
    }

    public override int turns { get { return durAdd + durMult * CubeOrient.SquareSize; } }

    public override void Apply(SnakeManager snakeManager)
    {
        foodInstance = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Food"));
        GameObject foodManager = GameObject.Find("FoodManager");
        foodInstance.transform.parent = foodManager.transform;
        FoodScript script = foodInstance.GetComponent<FoodScript>();
        script.duration = turns;
        script.points = points;
        script.gold = gold;
        script.color = foodColor;
        script.foodBehavior = FoodBehavior.Destroy;
        script.snakeHead = GameObject.Find("SnakeHead");
    }
}
