using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Reporting;
using UnityEngine;

public abstract class PermEffect
{
    public string abilityName { get; }
    public string description { get; }
    public string spritePath { get; }
    public Rarity rarity { get; }
    public int cost { get; }

    protected PermEffect(string abilityName, string description, string spriteFileName, Rarity rarity = Rarity.Common, int cost = 5)
    {
        this.abilityName = abilityName;
        this.description = description;
        spritePath = $"AbilityIcons/{spriteFileName}";
        this.rarity = rarity;
        this.cost = cost;
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
        Rarity.Common
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
        Rarity.Epic
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
        Rarity.Common
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
        Rarity.Rare
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
        Rarity.Common
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
        Rarity.Epic
    ) { }

    public override List<Effect> Effect(SnakeManager snakeManager)
    {
        return new List<Effect>{ new LengthModifier(0, 0.9f) };
    }
}

public class HardMode : PermEffect
{
    public HardMode() : base
    (
        "Hard Mode",
        "Curse of Adjacency",
        "Skull-Sprite",
        Rarity.Mythic
    ) { }

    public override List<Effect> Effect(SnakeManager snakeManager)
    {
        return new List<Effect>{ new COASpeed(0.15f), new FoodModifier(1.5f, 1), new LengthModifier(1, 0.66f) };
    }
}
