using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AnvilRecipeSingleUI : MonoBehaviour
{
    [SerializeField] private AnvilWorkstation anvilWorkstation;
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI recipeText;
    [SerializeField] private Button selectButton;
    [SerializeField] private Transform iconTemplate;
    [SerializeField] private GameObject partsGrid;
    private ForgingRecipeSO localForgingRecipeSO;

    private void Awake()
    {
        iconTemplate.gameObject.SetActive(false);

        selectButton.onClick.AddListener(() =>
        {
            anvilWorkstation.selectedForgingRecipeSO = localForgingRecipeSO;
            anvilWorkstation.SelectRecipeActions();
            GameManager.Instance.ToggleUsingCrafting();
        });
    }

    public void SetForgingRecipeSO(ForgingRecipeSO forgingRecipeSO)
    {
        image.sprite = forgingRecipeSO.output.sprite;
        recipeText.text = forgingRecipeSO.output.objectName;
        localForgingRecipeSO = forgingRecipeSO;
        //foreach gdyby by³o wiêcej ni¿ 1 input
        //foreach (WorkshopObjectSO workshopObjectSO in forgingRecipeSO.input)
        //{
            Transform iconTransform = Instantiate(iconTemplate, transform);
            iconTransform.SetParent(partsGrid.transform);
            iconTransform.gameObject.SetActive(true);
            iconTransform.GetComponent<CraftingIconsSingleUI>().SetWorkshopObjectSO(forgingRecipeSO.input);
        //}
    }

    public Button GetButton()
    {
        return selectButton;
    }
}
