using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Species", menuName = "Species")]
public class Species : ScriptableObject
{
    public string speciesName;
    public string qAbilityName;
    public string eAbilityName;
    public Material headMaterial;
    public List<Material> bodyMaterials;
    public List<Material> intermediaryMaterials;
}
