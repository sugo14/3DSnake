using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

public class PermEffectManager : MonoBehaviour
{
    public SnakeManager snakeManager;
    public GameObject effectsObject;
    public GameObject effectCellPrefab;

    public List<PermEffect> permEffects;

    public void AddEffect(PermEffect permEffect)
    {
        permEffects.Add(permEffect);
        AddCell(permEffect);
    }

    void AddCell(PermEffect permEffect)
    {
        GameObject effectCell = Instantiate(effectCellPrefab, effectsObject.transform);
        effectCell.GetComponent<UnityEngine.UI.Image>().sprite = Resources.Load<Sprite>(permEffect.spritePath);
        effectCell.GetComponent<UnityEngine.UI.Image>().color = permEffect.color;
    }

    void Start()
    {
        permEffects = new List<PermEffect>();
    }

    public void Reset()
    {
        foreach (PermEffect permEffect in permEffects)
        {
            permEffect.Apply(snakeManager);
        }
    }
}
