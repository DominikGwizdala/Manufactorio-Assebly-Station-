using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftingRecipeSingleUI : MonoBehaviour
{
    [SerializeField] private CraftingWorkstation craftingWorkstation;
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI recipeText;
    [SerializeField] private Button selectButton;
    [SerializeField] private Transform iconTemplate;
    [SerializeField] private GameObject partsGrid;
    private CraftingRecipeSO localCraftingRecipeSO;

    private void Awake()
    {
        iconTemplate.gameObject.SetActive(false);

        selectButton.onClick.AddListener(() =>
        {
            craftingWorkstation.selectedCraftingRecipeSO = localCraftingRecipeSO;
            craftingWorkstation.SelectRecipeActions();
            GameManager.Instance.ToggleUsingCrafting();
        });
    }

    public void SetCraftingRecipeSO(CraftingRecipeSO craftingRecipeSO)
    {
        image.sprite = craftingRecipeSO.output.sprite;
        recipeText.text = craftingRecipeSO.output.objectName;
        localCraftingRecipeSO = craftingRecipeSO;

        foreach (WorkshopObjectSO workshopObjectSO in craftingRecipeSO.inputSOList)
        {
            Transform iconTransform = Instantiate(iconTemplate, transform);
            iconTransform.SetParent(partsGrid.transform);
            iconTransform.gameObject.SetActive(true);
            iconTransform.GetComponent<CraftingIconsSingleUI>().SetWorkshopObjectSO(workshopObjectSO);
        }
    }

    public Button GetButton()
    {
        return selectButton;
    }
}
