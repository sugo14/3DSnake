using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public SnakeManager snakeManager;
    public int isInvincible = 0;

    public GameObject effectsObject;
    public GameObject effectCellPrefab;

    Dictionary<Effect, GameObject> effects = new Dictionary<Effect, GameObject>();

    public void AddEffect(Effect effect)
    {
        if (effect.hasAppearance)
        {
            GameObject effectCell = Instantiate(effectCellPrefab, effectsObject.transform);
            effectCell.GetComponentInChildren<UnityEngine.UI.Image>().sprite = Resources.Load<Sprite>("AbilityIcons/" + effect.spritePath);
            effectCell.GetComponentInChildren<UnityEngine.UI.Image>().color = snakeManager.snakeSpecies.snakeSpecies.headMaterial;
            effects.Add(effect, effectCell);
            effect.EditAppearance(effectCell);
        }
        else
        {
            effects.Add(effect, null);
        }
        effect.Apply(snakeManager);
    }

    public void RemoveEffect(Effect effect)
    {
        if (effects.ContainsKey(effect))
        {
            effect.Remove(snakeManager);
            if (effects[effect] != null)
            {
                Destroy(effects[effect]);
            }
            effects.Remove(effect);
        }
        else
        {
            Debug.LogError("Tried to remove effect that wasn't in the dictionary");
        }
    }

    void UpdateEffectAppearance()
    {
        foreach (Effect effect in effects.Keys)
        {
            if (effects[effect] != null)
            {
                effect.EditAppearance(effects[effect]);
            }
        }
    }

    public void OnTick()
    {
        for (int i = 0; i < effects.Count; i++)
        {
            Effect effect = effects.Keys.ElementAt(i);
            if (effect.WantsToDetach())
            {
                effect.Remove(snakeManager);
                effects.Remove(effect);
                i--;
            }
            effect.OnTick(snakeManager);
        }
        UpdateEffectAppearance();
    }

    public void Reset()
    {
        while (effects.Count > 0)
        {
            Effect effect = effects.Keys.ElementAt(0);
            RemoveEffect(effect);
        }
        effects.Clear();
    }
}
