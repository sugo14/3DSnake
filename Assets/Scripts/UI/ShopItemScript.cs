using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using System;

public enum ShopItemType
{
    Ability,
    PermEffect
}

public class ShopItemScript : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public ShopItemType itemType;
    public string itemName;

    public RectTransform rectTransform;
    public TMP_Text costText;
    public CanvasGroup canvasGroup;
    public float returnSpeed = 50;
    public UnityEngine.UI.Image image;

    Vector2 originalPosition;
    bool dragging = false;
    bool returning = false;
    Canvas canvas;

    void Start()
    {
        canvas = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Begin Drag");
        originalPosition = rectTransform.anchoredPosition;
        costText.enabled = false;
        dragging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        returning = false;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("End Drag");
        dragging = false;
        returning = true;
        canvasGroup.blocksRaycasts = true;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Pointer Down");
    }

    public void Initialize(ShopItemType itemType, string itemName)
    {
        this.itemType = itemType;
        this.itemName = itemName;

        int cost = 0;
        string path = "";
        Rarity rarity = Rarity.Common;

        if (itemType == ShopItemType.Ability)
        {
            Ability ability = AbilityRegistry.GetAbility(itemName);
            cost = ability.cost;
            path = ability.spritePath;
            rarity = ability.rarity;
        }
        else if (itemType == ShopItemType.PermEffect)
        {
            PermEffect permEffect = PermEffectRegistry.GetPermEffect(itemName);
            cost = permEffect.cost;
            path = permEffect.spritePath;
            rarity = permEffect.rarity;
        }

        GetComponentInChildren<TMP_Text>().text = "$" + cost.ToString();
        image.sprite = Resources.Load<Sprite>(path);
        image.color = GetComponentInParent<ShopScript>().rarityColors[(int)rarity];
    }

    void Update()
    {
        rectTransform.rotation = Quaternion.Lerp(rectTransform.rotation, dragging ? Quaternion.Euler(0, 0, Mathf.Sin(Time.time * 4) * 15) : Quaternion.Euler(0, 0, 0), 0.1f);
        if (returning) {
            rectTransform.anchoredPosition = Vector2.Lerp(rectTransform.anchoredPosition, originalPosition, returnSpeed * Time.deltaTime);
            if (Vector2.Distance(rectTransform.anchoredPosition, originalPosition) < 2f) {
                rectTransform.anchoredPosition = originalPosition;
                costText.enabled = true;
                returning = false;
            }
        }
    }

    public void Remove()
    {
        StartCoroutine(RemoveCoroutine());
    }

    IEnumerator RemoveCoroutine()
    {
        gameObject.SetActive(false);
        float currScale = transform.localScale.x;
        float targetScale = 0f;
        while (Math.Abs(currScale - targetScale) > 0.01f)
        {
            currScale = Mathf.Lerp(currScale, targetScale, 0.1f);
            transform.localScale = new Vector3(currScale, currScale, currScale);
            yield return null;
        }
        Destroy(gameObject);
    }
}
