using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CraftingIconsUI : MonoBehaviour
{
    [SerializeField] private CraftingWorkstation craftingWorkstation;
    [SerializeField] private TextMeshProUGUI selectedRecipeText;
    [SerializeField] private Transform iconTemplate;

    private void Awake()
    {
        iconTemplate.gameObject.SetActive(false);
    }

    private void Start()
    {
        UpdateVisual();
        craftingWorkstation.OnPartAdded += CraftingWorkstation_OnPartAdded;
    }

    private void CraftingWorkstation_OnPartAdded(object sender, CraftingWorkstation.OnPartAddedEventArgs e)
    {
        UpdateVisual();
    }

    public void UpdateVisual()
    {
        selectedRecipeText.text = "\nRecipe: " + craftingWorkstation.selectedRecipe;

        foreach (Transform child in transform)
        {
            if (child == iconTemplate) continue;
            Destroy(child.gameObject);
        }

        foreach (WorkshopObjectSO workshopObjectSO in craftingWorkstation.GetToRemoveSOList())
        {
            Transform iconTransform = Instantiate(iconTemplate, transform);
            iconTransform.gameObject.SetActive(true);
            iconTransform.GetComponent<PackageIconsSingleUI>().SetWorkshopObjectSO(workshopObjectSO);
        }
    }
}
