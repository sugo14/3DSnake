using System;
using UnityEngine;

public class UntimedEffect
{
    public virtual void OnTick() { }
    public virtual bool WantsToDetach() { return false; }
    public virtual void Apply(SnakeManager snakeManager) { }
    public virtual void Remove(SnakeManager snakeManager) { }
}

public class SpeedMultiplier : UntimedEffect
{
    public float speedMultiplier;

    public SpeedMultiplier(float speedMultiplier) { this.speedMultiplier = speedMultiplier; }

    public override void Apply(SnakeManager snakeManager) { snakeManager.tickTime /= speedMultiplier; }
    public override void Remove(SnakeManager snakeManager) { snakeManager.tickTime *= speedMultiplier; }
}

public class FoodModifier : UntimedEffect
{
    public float foodAdd, foodMult;

    public FoodModifier(float foodAdd, float foodMult)
    {
        this.foodAdd = foodAdd;
        this.foodMult = foodMult;
    }

    public override void Apply(SnakeManager snakeManager)
    {
        FoodManager fm = snakeManager.foodManager.GetComponent<FoodManager>();
        fm.foodAdd += foodAdd;
        fm.foodMult *= foodMult;
    }
    public override void Remove(SnakeManager snakeManager)
    {
        FoodManager fm = snakeManager.foodManager.GetComponent<FoodManager>();
        fm.foodAdd -= foodAdd;
        fm.foodMult /= foodMult;
    }
}

public class LengthModifier : UntimedEffect
{
    public int lengthAdd, lengthMult;

    public LengthModifier(int lengthAdd, int lengthMult)
    {
        this.lengthAdd = lengthAdd;
        this.lengthMult = lengthMult;
    }

    public override void Apply(SnakeManager snakeManager) { snakeManager.snakeMove.lengthMod += lengthAdd; }
    public override void Remove(SnakeManager snakeManager) { snakeManager.snakeMove.lengthMod -= lengthAdd; }
}
