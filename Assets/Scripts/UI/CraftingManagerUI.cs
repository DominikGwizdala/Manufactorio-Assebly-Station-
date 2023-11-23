using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class CraftingManagerUI : MonoBehaviour
{
    public static CraftingManagerUI Instance { get; private set; }

    [SerializeField] private Button pickaxeButton;
    [SerializeField] private Button axeButton;
    [SerializeField] private Button hammerButton;
    [SerializeField] private CraftingWorkstation craftingWorkstation;

    private void Awake()
    {
        Instance = this;

        pickaxeButton.onClick.AddListener(() => {
            Debug.Log("Wybrano kilof");
            craftingWorkstation.selectedRecipe = CraftingWorkstation.SelectedRecipe.Pickaxe;
            OnClickActions();
        });
        axeButton.onClick.AddListener(() => {
            Debug.Log("Wybrano siekiere");
            craftingWorkstation.selectedRecipe = CraftingWorkstation.SelectedRecipe.Axe;
            OnClickActions();
        });
        hammerButton.onClick.AddListener(() => {
            Debug.Log("Wybrano motyke");
            craftingWorkstation.selectedRecipe = CraftingWorkstation.SelectedRecipe.Hammer;
            OnClickActions();
        });
    }

    private void OnClickActions()
    {
        craftingWorkstation.isUsing = false;
        craftingWorkstation.SelectRecipe();
        GameManager.Instance.ToggleUsingCrafting();
    }
}
