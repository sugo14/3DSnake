using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Rarity
{
    Common,
    Rare,
    Epic,
    Mythic
}

public class ShopScript : MonoBehaviour
{
    public List<Color> rarityColors = new List<Color>()
    {
        Color.white,
        Color.blue,
        Color.yellow,
        Color.red
    };

    public List<string> shopItemNames = new List<string>(){ "Retract", "Freeze Frame", "Ghost" };

    public GameObject shopItems;
    public GameObject shopItemPrefab;

    public ShopAbilityScript qAbility, eAbility;

    public void Reset()
    {
        qAbility.Reset();
        eAbility.Reset();
    }
    
    void Start()
    {
        foreach (Transform child in shopItems.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (string shopItemName in shopItemNames)
        {
            if (AbilityRegistry.GetAbility(shopItemName) != null)
            {
                GameObject shopItem = Instantiate(shopItemPrefab, shopItems.transform);
                shopItem.GetComponent<ShopItemScript>().Initialize(ShopItemType.Ability, shopItemName);
            }
            else if (PermEffectRegistry.GetPermEffect(shopItemName) != null)
            {
                GameObject shopItem = Instantiate(shopItemPrefab, shopItems.transform);
                shopItem.GetComponent<ShopItemScript>().Initialize(ShopItemType.PermEffect, shopItemName);
            }
            else
            {
                Debug.LogError("Shop item " + shopItemName + " not found in AbilityRegistry or PermEffectRegistry");
            }
        }
    }

    void Update()
    {
        
    }
}
