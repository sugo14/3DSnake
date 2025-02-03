using System;
using UnityEngine;

public class FoodManager : MonoBehaviour
{
    public float foodAdd, foodMult = 1;

    public void Reset()
    {
        foreach (Transform child in transform)
        {
            FoodScript foodScript = child.GetComponent<FoodScript>();
            foodScript.RandomizeOrient();
        }
    }
}
