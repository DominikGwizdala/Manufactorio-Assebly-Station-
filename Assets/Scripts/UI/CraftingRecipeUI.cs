using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class CraftingRecipeUI : MonoBehaviour
{
    [SerializeField] private CraftingWorkstation craftingWorkstation;
    [SerializeField] private Transform recipeTemplate;
    private Button firstButton;

    private int selectButtonCounter = 0;

    private void Awake()
    {
        recipeTemplate.gameObject.SetActive(false);
    }

    private void Start()
    {
        foreach (Transform child in transform)
        {
            if (child == recipeTemplate) continue;
            Destroy(child.gameObject);
        }

        foreach (CraftingRecipeSO craftingRecipeSO in craftingWorkstation.GetCraftingRecipeSOArray())
        {
            Transform recipeTransform = Instantiate(recipeTemplate, transform);
            recipeTransform.gameObject.SetActive(true);
            recipeTransform.GetComponent<CraftingRecipeSingleUI>().SetCraftingRecipeSO(craftingRecipeSO);
            if (selectButtonCounter == 0)
            {
                firstButton = recipeTransform.GetComponent<CraftingRecipeSingleUI>().GetButton();
                firstButton.Select();
            }
            selectButtonCounter++;
        }
    }

    public void SelectFirstButton()
    {
        firstButton.Select();
    }
}
