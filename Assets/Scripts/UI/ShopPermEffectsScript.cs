using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShopPermEffectsScript : MonoBehaviour, IDropHandler
{
    public GameObject permEffectPrefab;
    public PermEffectManager permEffectManager;
    public ShopScript shopScript;

    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        ShopItemScript shopItemScript = dropped.GetComponent<ShopItemScript>();

        if (shopItemScript.itemType == ShopItemType.PermEffect)
        {
            permEffectManager.AddEffect(PermEffectRegistry.GetPermEffect(shopItemScript.itemName));
            shopItemScript.Remove();
            UpdateVisuals();
        }
    }

    void Initialize(GameObject gameObject, PermEffect permEffect)
    {
        UnityEngine.UI.Image image = gameObject.GetComponent<UnityEngine.UI.Image>();
        image.color = shopScript.rarityColors[(int)permEffect.rarity];
        image.sprite = Resources.Load<Sprite>(permEffect.spritePath);
    }

    public void UpdateVisuals()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        foreach (PermEffect permEffect in permEffectManager.permEffects)
        {
            GameObject effectObject = Instantiate(permEffectPrefab, transform);
            Initialize(effectObject, permEffect);
        }
    }
}
