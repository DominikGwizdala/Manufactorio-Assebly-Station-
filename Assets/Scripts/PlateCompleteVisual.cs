using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlateCompleteVisual;

public class PlateCompleteVisual : MonoBehaviour
{
    [Serializable] public struct WorkshopObjectSO_GameObject
    {
        public WorkshopObjectSO workshopObjectSO;
        public GameObject gameObject;
    }

    [SerializeField] private PlateWorkshopObject plateWorkshopObject;
    [SerializeField] private List<WorkshopObjectSO_GameObject> workshopObjectSOGameObjectList;

    private void Start()
    {
        plateWorkshopObject.OnIngredientAdded += PlateWorkshopObject_OnIngredientAdded;

        foreach (WorkshopObjectSO_GameObject workshopObjectSOGameObject in workshopObjectSOGameObjectList)
        {
            workshopObjectSOGameObject.gameObject.SetActive(false);
        }
    }

    private void PlateWorkshopObject_OnIngredientAdded(object sender, PlateWorkshopObject.OnIngredientAddedEventArgs e)
    {
        foreach (WorkshopObjectSO_GameObject workshopObjectSOGameObject in workshopObjectSOGameObjectList)
        {
            if (workshopObjectSOGameObject.workshopObjectSO == e.workshopObjectSO)
            {
                workshopObjectSOGameObject.gameObject.SetActive(true);
            }
        }
    }
}
