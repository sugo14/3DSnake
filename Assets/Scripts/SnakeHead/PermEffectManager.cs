using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

public class PermEffectManager : MonoBehaviour
{
    public SnakeManager snakeManager;

    public List<PermEffect> permEffects = new List<PermEffect>();

    public void AddEffect(PermEffect permEffect)
    {
        permEffects.Add(permEffect);
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
