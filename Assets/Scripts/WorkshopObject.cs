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

    public bool TryGetPackage(out PackageWorkshopObject packageWorkshopObject)
    {
        if (this is PackageWorkshopObject)
        {
            packageWorkshopObject = this as PackageWorkshopObject;
            return true;
        } 
        else
        {
            packageWorkshopObject = null;
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
