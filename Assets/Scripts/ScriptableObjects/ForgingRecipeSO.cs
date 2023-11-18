using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class ForgingRecipeSO : ScriptableObject
{
    public WorkshopObjectSO input;
    public WorkshopObjectSO output;
    public int forgingProgressMax;
}
