using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class CraftingRecipeSO : ScriptableObject
{
    public List<WorkshopObjectSO> inputSOList;
    public WorkshopObjectSO output;
    public string recipeName;
}
