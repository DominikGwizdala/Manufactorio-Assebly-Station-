using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackageIconsUI : MonoBehaviour
{
    [SerializeField] private PackageWorkshopObject packageWorkshopObject;
    [SerializeField] private Transform iconTemplate;

    private void Awake()
    {
        iconTemplate.gameObject.SetActive(false);
    }

    private void Start()
    {
        packageWorkshopObject.OnPartAdded += PackageWorkshopObject_OnPartAdded;
    }

    private void PackageWorkshopObject_OnPartAdded(object sender, PackageWorkshopObject.OnPartAddedEventArgs e)
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

        foreach (WorkshopObjectSO workshopObjectSO in packageWorkshopObject.GetWorkshopObjectSOList())
        {
            Transform iconTransform = Instantiate(iconTemplate, transform);
            iconTransform.gameObject.SetActive(true);
            iconTransform.GetComponent<CraftingIconsSingleUI>().SetWorkshopObjectSO(workshopObjectSO);
        }
    }
}
