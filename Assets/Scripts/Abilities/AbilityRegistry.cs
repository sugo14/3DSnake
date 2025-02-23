using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AbilityRegistry
{
    private static Dictionary<string, Ability> abilities = new Dictionary<string, Ability>();

    static AbilityRegistry()
    {
        RegisterAbility(new FreezeFrame());
        RegisterAbility(new Ghost());
        RegisterAbility(new Hourglass());
        RegisterAbility(new Snip());
        RegisterAbility(new Teleport());
        RegisterAbility(new Bomb());
        RegisterAbility(new LineCollect());
        RegisterAbility(new FaceCollect());
        RegisterAbility(new Retract());
        RegisterAbility(new Spawn());
    }

    public static void RegisterAbility(Ability ability)
    {
        abilities[ability.abilityName] = ability;
    }

    public static Ability GetAbility(string abilityName)
    {
        if (!abilities.ContainsKey(abilityName))
        {
            return null;
        }
        return abilities[abilityName];
    }

    public static List<string> GetAbilityNames()
    {
        return new List<string>(abilities.Keys);
    }

    public static List<Ability> GetAbilities()
    {
        return new List<Ability>(abilities.Values);
    }
}
