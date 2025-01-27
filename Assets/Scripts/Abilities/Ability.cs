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
        20
    ) { }

    public override List<Effect> Effect(SnakeManager snakeManager) { return new List<Effect>{new SpeedChange(4, 0.4f)}; }
}

public class Ghost : Ability
{
    public Ghost() : base
    (
        "Ghost",
        "Invincible to self crashes",
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
        "Use your other ability",
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
        "Reduce length by 15 temporarily",
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
        "Instantly move 4 spaces forward",
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
        "Reduce length by 30 temporarily and score by 10",
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
        "Collect all food in a line in front of you",
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
        "Collect all food on your current cube face",
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
        "Move back 7 spaces",
        "Retract-Sprite",
        30
    ) { }

    public override List<Effect> Effect(SnakeManager snakeManager) { return new List<Effect>{new Reverse(7)}; }
}
