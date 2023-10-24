using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackageWorkshopObject : WorkshopObject
{
    public event EventHandler<OnPartAddedEventArgs> OnPartAdded;
    public class OnPartAddedEventArgs: EventArgs
    {
        public WorkshopObjectSO workshopObjectSO;
    }

    [SerializeField] private List<WorkshopObjectSO> validWorkshopObjectSOList;

    private List<WorkshopObjectSO> workshopObjectSOList;

    private void Awake()
    {
        workshopObjectSOList = new List<WorkshopObjectSO>();
    }

    public bool TryAddPart(WorkshopObjectSO workshopObjectSO)
    {
        if (!validWorkshopObjectSOList.Contains(workshopObjectSO))
        {
            return false;
        }
        if (workshopObjectSOList.Contains(workshopObjectSO))
        {
            return false;
        }
        else
        {
            workshopObjectSOList.Add(workshopObjectSO);
            OnPartAdded?.Invoke(this, new OnPartAddedEventArgs
            {
                workshopObjectSO = workshopObjectSO
            });
            return true;
        }
    }

    public List<WorkshopObjectSO> GetWorkshopObjectSOList()
    {
        return workshopObjectSOList;
    }
}
