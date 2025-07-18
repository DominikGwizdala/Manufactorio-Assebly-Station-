using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWorkshopObjectParent
{
    public Transform GetWorkshopObjectFollowTransform();
    public void SetWorkshopObject(WorkshopObject workshopObject);
    public WorkshopObject GetWorkshopObject();
    public void ClearWorkshopObject();
    public bool HasWorkshopObject();
}
