using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateWorkshopObject : WorkshopObject
{
    public event EventHandler<OnIngredientAddedEventArgs> OnIngredientAdded;
    public class OnIngredientAddedEventArgs: EventArgs
    {
        public WorkshopObjectSO workshopObjectSO;
    }

    [SerializeField] private List<WorkshopObjectSO> validWorkshopObjectSOList;

    private List<WorkshopObjectSO> workshopObjectSOList;

    private void Awake()
    {
        workshopObjectSOList = new List<WorkshopObjectSO>();
    }

    public bool TryAddIngredient(WorkshopObjectSO workshopObjectSO)
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
            OnIngredientAdded?.Invoke(this, new OnIngredientAddedEventArgs
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
