using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SnakePreview : MonoBehaviour
{
    public GameObject snakePreview;
    public TMP_Text speciesNameText;
    public TMP_Text speciesDescriptionText;

    public SpeciesRegistry speciesRegistry;

    public float rotateSpeed = 0.2f;

    public int currentSpeciesIndex = 0;

    void Start()
    {
        UpdateSnakePreview();
    }

    void UpdateSnakePreview()
    {
        snakePreview.GetComponent<MeshRenderer>().material = speciesRegistry.speciesList[currentSpeciesIndex].bodyMaterials[0];
        speciesNameText.text = speciesRegistry.speciesList[currentSpeciesIndex].speciesName;
        Ability ability = AbilityRegistry.GetAbility(speciesRegistry.speciesList[currentSpeciesIndex].qAbilityName);
        speciesDescriptionText.text = ability == null ? "No ability." : ability.description;
    }

    void Update()
    {
        snakePreview.transform.Rotate(0, rotateSpeed, 0);

        if (Input.GetKeyDown(KeyCode.A))
        {
            currentSpeciesIndex = (currentSpeciesIndex - 1 + speciesRegistry.speciesList.Count) % speciesRegistry.speciesList.Count;
            UpdateSnakePreview();
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            currentSpeciesIndex = (currentSpeciesIndex + 1) % speciesRegistry.speciesList.Count;
            UpdateSnakePreview();
        }
    }
}
