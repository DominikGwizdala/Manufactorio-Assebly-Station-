using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseWorkstation : MonoBehaviour, IWorkshopObjectParent
{
    public static event EventHandler OnAnyObjectPlacedHere;

    public static void ResetStaticData()
    {
        OnAnyObjectPlacedHere = null;
    }
    [SerializeField] private Transform workstationTopPoint;

    private WorkshopObject workshopObject;

    public virtual void Interact(Player player)
    {
        Debug.LogError("BaseWorkstation interaction");
    }
    public virtual void InteractAlternate(Player player)
    {
        //Debug.LogError("BaseWorkstation interactionAlternate");
    }
    public Transform GetWorkshopObjectFollowTransform()
    {
        return workstationTopPoint;
    }
    public void SetWorkshopObject(WorkshopObject workshopObject)
    {
        this.workshopObject = workshopObject;
        if (workshopObject != null )
        {
            OnAnyObjectPlacedHere?.Invoke(this, EventArgs.Empty);
        }
    }
    public WorkshopObject GetWorkshopObject()
    {
        return workshopObject;
    }
    public void ClearWorkshopObject()
    {
        workshopObject = null;
    }
    public bool HasWorkshopObject()
    {
        return workshopObject != null;
    }
}
