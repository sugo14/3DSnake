using System.Collections.Generic;
using UnityEngine;

public abstract class Ability
{
    public string abilityName { get; }
    public string description { get; }
    public string spritePath { get; }
    public int cooldown { get; }

    protected Ability(string abilityName, string description, string spriteFileName, int cooldown)
    {
        this.abilityName = abilityName;
        this.description = description;
        spritePath = $"AbilityIcons/{spriteFileName}";
        this.cooldown = cooldown;
    }

    public abstract List<Effect> Effect(SnakeManager snakeManager);

    public int GetDuration(SnakeManager snakeManager)
    {
        List<Effect> effects = Effect(snakeManager);
        int duration = 0;
        foreach (Effect effect in effects)
        {
            if (effect.turns > duration) { duration = effect.turns; }
        }
        return duration;
    }

    public AbilityInstance Instantiate()
    {
        return new AbilityInstance(this);
    }
}

public class FreezeFrame : Ability
{
    public FreezeFrame() : base
    (
        "Freeze Frame",
        "Slow time to 0.4x speed",
        "Clock-Sprite",
        15
    ) { }

    public override List<Effect> Effect(SnakeManager snakeManager) { return new List<Effect>{new SpeedChange(4, 0.4f)}; }
}

public class Ghost : Ability
{
    public Ghost() : base
    (
        "Ghost",
        "Provides invincibility to crashes into the body for 3 turns.",
        "Ghost-Sprite",
        40
    ) { }

    public override List<Effect> Effect(SnakeManager snakeManager) { return new List<Effect>{new Invincibility(3)}; }
}

public class Hourglass : Ability
{
    public Hourglass() : base
    (
        "Hourglass",
        "Copies your other equipped ability, or none if no other ability is equipped.",
        "Hourglass-Sprite",
        60
    ) { }

    public override List<Effect> Effect(SnakeManager snakeManager) {
        Ability otherAbility = snakeManager.abilities.qAbility;
        if (otherAbility == this || otherAbility == null) { otherAbility = snakeManager.abilities.eAbility; }
        if (otherAbility == this || otherAbility == null) { return new List<Effect>(); }
        return otherAbility.Effect(snakeManager);
    }
}

public class Snip : Ability
{
    public Snip() : base
    (
        "Snip",
        "Temporarily reduces the snake's length by 15, slowly growing back.",
        "Length-Reduce-Sprite",
        30
    ) { }

    public override List<Effect> Effect(SnakeManager snakeManager) { return new List<Effect>{new ReduceLength(15)}; }
}

public class Teleport : Ability
{
    public Teleport() : base
    (
        "Teleport",
        "Instantly moves the snake head 4 spaces forward, leaving part of the body behind.",
        "Teleport-Sprite",
        40
    ) { }

    public override List<Effect> Effect(SnakeManager snakeManager) { return new List<Effect>{new MoveForward(4)}; }
}

public class Bomb : Ability
{
    public Bomb() : base
    (
        "Bomb",
        "Reduces score by 10. Temporarily reduces length by 30, slowly growing back.",
        "Bomb-Sprite",
        60
    ) { }

    public override List<Effect> Effect(SnakeManager snakeManager) { return new List<Effect>{new ReduceLength(30), new RemoveScore(10)}; }
}

public class LineCollect : Ability
{
    public LineCollect() : base
    (
        "Line Collect",
        "Collects all of the food in a line in front of the snake head, wrapping around the cube.",
        "Arrow-Sprite",
        10
    ) { }

    public override List<Effect> Effect(SnakeManager snakeManager) { return new List<Effect>{new CollectFoodInLine()}; }
}

public class FaceCollect : Ability
{
    public FaceCollect() : base
    (
        "Face Collect",
        "Collects all of the food on the same face of the cube as the snake head.",
        "Face-Sprite",
        15
    ) { }

    public override List<Effect> Effect(SnakeManager snakeManager) { return new List<Effect>{new CollectFoodOnFace()}; }
}

public class Retract : Ability
{
    public Retract() : base
    (
        "Retract",
        "The snake head retracts into the body by 7 spaces, slowly growing back.",
        "Retract-Sprite",
        30
    ) { }

    public override List<Effect> Effect(SnakeManager snakeManager) { return new List<Effect>{new Reverse(7), new SpeedChange(2, 0.4f)}; }
}
