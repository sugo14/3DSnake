using UnityEngine;

public class FoodManager : MonoBehaviour
{
    public void Reset()
    {
        foreach (Transform child in transform)
        {
            FoodScript foodScript = child.GetComponent<FoodScript>();
            foodScript.RandomizeOrient();
        }
    }
}
