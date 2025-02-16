using System.Collections.Generic;
using UnityEngine;

public class PermEffectManager : MonoBehaviour
{
    public SnakeManager snakeManager;

    public List<PermEffect> permEffects = new List<PermEffect>();

    public void AddEffect(PermEffect permEffect)
    {
        permEffects.Add(permEffect);
        permEffect.Apply(snakeManager);
    }

    void Start()
    {
        Reset();
    }

    public void Reset()
    {
        foreach (PermEffect permEffect in permEffects)
        {
            permEffect.Apply(snakeManager);
        }
        foreach (string permEffectName in snakeManager.snakeSpecies.snakeSpecies.permEffectNames)
        {
            PermEffect permEffect = PermEffectRegistry.GetPermEffect(permEffectName);
            if (permEffect != null)
            {
                permEffect.Apply(snakeManager);
            }
        }
    }
}
