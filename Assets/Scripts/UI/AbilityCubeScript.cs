using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AbilityCubeScript : MonoBehaviour
{
    public float sizeChange = 0.1f, sizeSpeed = 4, rotationDur = 0.2f;

    bool applied = true;

    float currRotation = 0;
    bool isRotating = false;
    float rotationSpeed = 0;

    void Start()
    {
        SetFillAmount(1);
        SetApplied(true);
        SetText("");
    }

    public void SetFillAmount(float amount)
    {
        int deg = 360 - (int)(amount * 360);
        for (int i = 0; i < 6; i++)
        {
            transform.GetChild(i).gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().material.SetFloat("_Arc2", deg);
        }
    }

    public void SetApplied(bool applied)
    {
        this.applied = applied;
        for (int i = 0; i < 6; i++)
        {
            transform.GetChild(i).gameObject.transform.GetChild(1).gameObject.SetActive(!applied);
        }
    }

    public void SetText(string text)
    {
        for (int i = 0; i < 6; i++)
        {
            transform.GetChild(i).gameObject.transform.GetChild(2).GetComponent<TMP_Text>().text = text;
        }
    }

    public void SetSprite(Sprite sprite)
    {
        for (int i = 0; i < 6; i++)
        {
            transform.GetChild(i).gameObject.transform.GetChild(3).GetComponent<SpriteRenderer>().sprite = sprite;
            transform.GetChild(i).gameObject.transform.GetChild(3).GetComponent<AutoResizeSprite>().Resize();
        }
    }

    public void SetColor(Color color)
    {
        for (int i = 0; i < 6; i++)
        {
            transform.GetChild(i).gameObject.transform.GetChild(3).GetComponent<SpriteRenderer>().color = color;
        }
    }

    public void Activate() { isRotating = true; }

    void Update()
    {
        if (applied)
        {
            float scale = 1f + sizeChange + Mathf.Sin(Time.time * sizeSpeed) * sizeChange;
            gameObject.transform.localScale = Vector3.Lerp(gameObject.transform.localScale, new Vector3(scale, scale, scale), 0.1f);
        }
        else
        {
            gameObject.transform.localScale = Vector3.Lerp(gameObject.transform.localScale, Vector3.one, 0.1f);
        }

        if (isRotating)
        {
            currRotation = Mathf.SmoothDamp(currRotation, 360, ref rotationSpeed, rotationDur);
            transform.rotation = Quaternion.Euler(currRotation, 0, 0);
            if (currRotation > 359)
            {
                isRotating = false;
                currRotation = 0;
                transform.rotation = Quaternion.identity;
            }
        }
    }
}
