using UnityEngine;
using UnityEngine.EventSystems;

public class ShopAbilityScript : MonoBehaviour, IDropHandler
{
    public SnakeManager snakeManager;
    public UnityEngine.UI.Image sprite;
    public UnityEngine.UI.Image inactiveCover;
    public ShopScript shopScript;
    public bool isQ = false;

    public void Reset() { UpdateVisuals(); }

    void UpdateVisuals()
    {
        Ability ability = isQ ? snakeManager.abilities.qAbility : snakeManager.abilities.eAbility;
        if (ability == null)
        {
            sprite.sprite = null;
            inactiveCover.gameObject.SetActive(true);
            return;
        }
        sprite.color = shopScript.rarityColors[(int)ability.rarity];
        inactiveCover.gameObject.SetActive(false);
        string path = ability.spritePath;
        sprite.sprite = (path == "") ? null : Resources.Load<Sprite>(path);
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("OnDrop");
        GameObject dropped = eventData.pointerDrag;
        ShopItemScript shopItemScript = dropped.GetComponent<ShopItemScript>();

        if (shopItemScript.itemType == ShopItemType.Ability)
        {
            if (isQ)
            {
                snakeManager.abilities.SetQAbility(AbilityRegistry.GetAbility(shopItemScript.itemName));
            }
            else
            {
                snakeManager.abilities.SetEAbility(AbilityRegistry.GetAbility(shopItemScript.itemName));
            }
            shopItemScript.Remove();
            UpdateVisuals();
        }
    }
}
