using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class SmeltingRecipeSO : ScriptableObject
{
    public WorkshopObjectSO input;     
    public WorkshopObjectSO output;
    public float smeltingTimerMax;
}
