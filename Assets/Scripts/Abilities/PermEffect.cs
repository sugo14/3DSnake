using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PermEffect
{
    public string abilityName { get; }
    public string description { get; }
    public string spritePath { get; }
    public Color color { get; }

    protected PermEffect(string abilityName, string description, string spriteFileName, Color color)
    {
        this.abilityName = abilityName;
        this.description = description;
        spritePath = $"AbilityIcons/{spriteFileName}";
        this.color = color;
    }

    public abstract List<Effect> Effect(SnakeManager snakeManager);

    public void Apply(SnakeManager snakeManager)
    {
        List<Effect> effects = Effect(snakeManager);
        foreach (Effect effect in effects)
        {
            snakeManager.effectManager.AddEffect(effect);
        }
    }
}

public class SlowDown : PermEffect
{

    public SlowDown() : base
    (
        "Slow Down",
        "Sets speed to 0.95x",
        "Clock-Sprite",
        Color.cyan
    ) { }

    public override List<Effect> Effect(SnakeManager snakeManager)
    {
        return new List<Effect>{ new SpeedMultiplier(0.95f) };
    }
}

public class GreaterSlowDown : PermEffect
{

    public GreaterSlowDown() : base
    (
        "Greater Slow Down",
        "Sets speed to 0.9x",
        "Clock-Sprite",
        Color.yellow
    ) { }

    public override List<Effect> Effect(SnakeManager snakeManager)
    {
        return new List<Effect>{ new SpeedMultiplier(0.9f) };
    }
}

public class FoodAdd : PermEffect
{

    public FoodAdd() : base
    (
        "Food Add",
        "+1 point per food",
        "Growth-Sprite",
        Color.red
    ) { }

    public override List<Effect> Effect(SnakeManager snakeManager)
    {
        return new List<Effect>{ new FoodModifier(1, 1) };
    }
}

public class FoodMult : PermEffect
{

    public FoodMult() : base
    (
        "Food Mult",
        "1.25x food points",
        "Growth-Sprite",
        Color.yellow
    ) { }

    public override List<Effect> Effect(SnakeManager snakeManager)
    {
        return new List<Effect>{ new FoodModifier(0, 1.25f) };
    }
}

public class LengthCut : PermEffect
{
    public LengthCut() : base
    (
        "Length Cut",
        "-5 length permanently",
        "Scissors-Sprite",
        Color.green
    ) { }

    public override List<Effect> Effect(SnakeManager snakeManager)
    {
        return new List<Effect>{ new LengthModifier(-5, 1) };
    }
}

public class LengthMult : PermEffect
{
    public LengthMult() : base
    (
        "Length Mult",
        "0.9x length permanently",
        "Scissors-Sprite",
        Color.yellow
    ) { }

    public override List<Effect> Effect(SnakeManager snakeManager)
    {
        return new List<Effect>{ new LengthModifier(0, 0.9f) };
    }
}
