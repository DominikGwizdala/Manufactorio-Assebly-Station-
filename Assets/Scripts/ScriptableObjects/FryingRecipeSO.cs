using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class FryingRecipeSO :ScriptableObject
{
    public WorkshopObjectSO input;     
    public WorkshopObjectSO output;
    public float fryingTimerMax;
}
