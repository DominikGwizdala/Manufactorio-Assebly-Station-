using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class AnvilRecipeUI : MonoBehaviour
{
    [SerializeField] private AnvilWorkstation anvilWorkstation;
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

        foreach (ForgingRecipeSO forgingRecipeSO in anvilWorkstation.GetForgingRecipeSOArray())
        {
            Transform recipeTransform = Instantiate(recipeTemplate, transform);
            recipeTransform.gameObject.SetActive(true);
            recipeTransform.GetComponent<AnvilRecipeSingleUI>().SetForgingRecipeSO(forgingRecipeSO);
            if (selectButtonCounter == 0)
            {
                firstButton = recipeTransform.GetComponent<AnvilRecipeSingleUI>().GetButton();
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
