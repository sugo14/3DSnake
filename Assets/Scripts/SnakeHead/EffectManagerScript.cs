using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public SnakeManager snakeManager;
    public int isInvincible = 0;

    List<Effect> effects = new List<Effect>();

    public void OnTick()
    {
        for (int i = 0; i < effects.Count; i++)
        {
            Effect effect = effects[i];
            if (effect.WantsToDetach())
            {
                effect.Remove(snakeManager);
                effects.Remove(effect);
                i--;
            }
            effect.OnTick();
        }
    }

    public void AddEffect(Effect effect)
    {
        effect.Apply(snakeManager);
        effects.Add(effect);
    }

    public void Reset()
    {
        foreach (Effect effect in effects)
        {
            effect.Remove(snakeManager);
        }
        effects.Clear();
    }
}
