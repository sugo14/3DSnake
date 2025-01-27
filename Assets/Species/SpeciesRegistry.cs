using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Species Registry", menuName = "Species Registry")]
public class SpeciesRegistry : ScriptableObject
{
    public List<Species> speciesList;
}
