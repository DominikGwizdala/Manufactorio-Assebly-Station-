using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateIconsUI : MonoBehaviour
{
    [SerializeField] private PlateWorkshopObject plateWorkshopObject;
    [SerializeField] private Transform iconTemplate;

    private void Awake()
    {
        iconTemplate.gameObject.SetActive(false);
    }

    private void Start()
    {
        plateWorkshopObject.OnIngredientAdded += PlateWorkshopObject_OnIngredientAdded;
    }

    private void PlateWorkshopObject_OnIngredientAdded(object sender, PlateWorkshopObject.OnIngredientAddedEventArgs e)
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        foreach (Transform child in transform)
        {
            if (child == iconTemplate) continue;
            Destroy(child.gameObject);
        }

        foreach (WorkshopObjectSO workshopObjectSO in plateWorkshopObject.GetWorkshopObjectSOList())
        {
            Transform iconTransform = Instantiate(iconTemplate, transform);
            iconTransform.gameObject.SetActive(true);
            iconTransform.GetComponent<PlateIconsSingleUI>().SetWorkshopObjectSO(workshopObjectSO);
        }
    }
}
