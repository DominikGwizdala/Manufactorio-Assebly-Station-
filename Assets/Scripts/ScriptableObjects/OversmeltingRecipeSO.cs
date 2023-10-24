using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class OversmeltingRecipeSO : ScriptableObject
{
    public WorkshopObjectSO input;     
    public WorkshopObjectSO output;
    public float oversmeltingTimerMax;
}
