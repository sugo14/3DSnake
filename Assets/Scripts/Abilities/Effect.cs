using System;
using UnityEngine;

public abstract class Effect {
    public int turns;

    public Effect(int turns) { this.turns = turns; }

    public virtual void OnTick() { turns--; }
    public virtual bool WantsToDetach() { return turns <= 0; }

    public virtual void Apply(SnakeManager snakeManager) { }
    public virtual void Remove(SnakeManager snakeManager) { }
}

public class None : Effect {
    public None() : base(0) { }
}

public class SpeedChange : Effect {
    public float speedMultiplier;

    public SpeedChange(int turns, float speedMultiplier) : base(turns)
    {
        this.speedMultiplier = speedMultiplier;
    }

    public override void Apply(SnakeManager snakeManager) { snakeManager.tickTime /= speedMultiplier; }
    public override void Remove(SnakeManager snakeManager) { snakeManager.tickTime *= speedMultiplier; }
}

public class Invincibility : Effect {
    public Invincibility(int turns) : base(turns) { }

    public override void Apply(SnakeManager snakeManager) { snakeManager.effectManager.isInvincible++; }
    public override void Remove(SnakeManager snakeManager) { snakeManager.effectManager.isInvincible--; }
}

public class ReduceLength : Effect {
    public int length;

    public ReduceLength(int length) : base(1)
    {
        this.length = length;
    }

    public override void Apply(SnakeManager snakeManager) { snakeManager.snakeMove.lengthMod -= length; }
    public override void Remove(SnakeManager snakeManager) { snakeManager.snakeMove.lengthMod += length; }
}

public class MoveForward : Effect {
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

public class RemoveScore : Effect
{
    public int score;

    public RemoveScore(int score) : base(1)
    {
        this.score = score;
    }

    public override void Apply(SnakeManager snakeManager) { snakeManager.snakeMove.currLength = Math.Max(snakeManager.snakeMove.currLength - score, 0); }
}

public class CollectFoodInLine : Effect
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

public class CollectFoodOnFace : Effect
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

public class GainPoints : Effect
{
    public int points;

    public GainPoints(int points) : base(1)
    {
        this.points = points;
    }

    public override void Apply(SnakeManager snakeManager) { snakeManager.snakeMove.currLength += points; }
}

public class Reverse : Effect
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
            GameObject lastBody = snakeManager.snakeMove.snakeBody[0];
            snakeManager.snakeMove.snakeBody.RemoveAt(0);
            GameObject.Destroy(lastBody);
        }
        GameObject mostRecent = snakeManager.snakeMove.snakeBody[0];
        snakeManager.snakeMove.orient = CubeOrient.Copy(mostRecent.GetComponent<SnakeBody>().cubeOrient);
        snakeManager.transform.position = snakeManager.snakeMove.orient.WorldPosition();
    }
}
