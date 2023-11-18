using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class CraftingRecipeSO : ScriptableObject
{
    public WorkshopObjectSO input;
    public WorkshopObjectSO output;
}
