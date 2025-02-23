using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PermEffectRegistry
{
    private static Dictionary<string, PermEffect> permEffects = new Dictionary<string, PermEffect>();

    static PermEffectRegistry()
    {
        RegisterPermEffect(new SlowDown());
        RegisterPermEffect(new GreaterSlowDown());
        RegisterPermEffect(new FoodAdd());
        RegisterPermEffect(new FoodMult());
        RegisterPermEffect(new LengthCut());
        RegisterPermEffect(new LengthMult());
        RegisterPermEffect(new HardMode());
        RegisterPermEffect(new AddedFood());
    }

    public static void RegisterPermEffect(PermEffect permEffect)
    {
        permEffects[permEffect.abilityName] = permEffect;
    }

    public static PermEffect GetPermEffect(string permEffectName)
    {
        if (!permEffects.ContainsKey(permEffectName))
        {
            return null;
        }
        return permEffects[permEffectName];
    }

    public static List<string> GetPermEffectNames()
    {
        return new List<string>(permEffects.Keys);
    }

    public static List<PermEffect> GetPermEffects()
    {
        return new List<PermEffect>(permEffects.Values);
    }
}