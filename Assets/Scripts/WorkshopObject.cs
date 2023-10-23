using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkshopObject : MonoBehaviour
{
    [SerializeField] private WorkshopObjectSO workshopObjectSO;

    private IWorkshopObjectParent workshopObjectParent;
    public WorkshopObjectSO GetWorkshopObjectSO() { 
        return workshopObjectSO;
    }
    public void SetWorkshopObjectParent(IWorkshopObjectParent workshopObjectParent)
    {
        if (this.workshopObjectParent != null)
        {
            this.workshopObjectParent.ClearWorkshopObject();
        }

        this.workshopObjectParent = workshopObjectParent;        

        if (workshopObjectParent.HasWorkshopObject())
        {
            Debug.LogError("IWorkshopObjectParent ma ju¿ obiekt");
        }

        workshopObjectParent.SetWorkshopObject(this);

        transform.parent = workshopObjectParent.GetWorkshopObjectFollowTransform();
        transform.localPosition = Vector3.zero;
    }
    public IWorkshopObjectParent GetWorkshopObjectParent()
    {
        return workshopObjectParent;
    }

    public void DestroySelf() {
        workshopObjectParent.ClearWorkshopObject() ;
        Destroy(gameObject);
    }

    public bool TryGetPlate(out PlateWorkshopObject plateWorkshopObject)
    {
        if (this is PlateWorkshopObject)
        {
            plateWorkshopObject = this as PlateWorkshopObject;
            return true;
        } 
        else
        {
            plateWorkshopObject = null;
            return false;
        }
    }

    public static WorkshopObject SpawnWorkshopObject(WorkshopObjectSO workshopObjectSO, IWorkshopObjectParent workshopObjectParent)
    {
        Transform workshopObjectTransform = Instantiate(workshopObjectSO.prefab);
        WorkshopObject workshopObject = workshopObjectTransform.GetComponent<WorkshopObject>();
        workshopObject.SetWorkshopObjectParent(workshopObjectParent);
        return workshopObject;
    }
}
