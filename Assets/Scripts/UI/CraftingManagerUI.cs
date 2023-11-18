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
    [SerializeField] private Button hoeButton;
    [SerializeField] private CraftingWorkstation craftingWorkstation;

    private void Awake()
    {
        Instance = this;

        pickaxeButton.onClick.AddListener(() => {
            Debug.Log("Wybrano kilof");
            craftingWorkstation.selectedRecipe = CraftingWorkstation.SelectedRecipe.Pickaxe;
            craftingWorkstation.isUsing = false;
            Hide();
            GameManager.Instance.ToggleUsingCrafting();
        });
        axeButton.onClick.AddListener(() => {
            Debug.Log("Wybrano siekiere");
            craftingWorkstation.selectedRecipe = CraftingWorkstation.SelectedRecipe.Axe;
            craftingWorkstation.isUsing = false;
            Hide();
            GameManager.Instance.ToggleUsingCrafting();
        });
        hoeButton.onClick.AddListener(() => {
            Debug.Log("Wybrano motyke");
            craftingWorkstation.selectedRecipe = CraftingWorkstation.SelectedRecipe.Hoe;
            craftingWorkstation.isUsing = false;
            Hide();
            GameManager.Instance.ToggleUsingCrafting();
        });
    }
    private void Start()
    {
        Hide();
    }
    public void Show()
    {
        gameObject.SetActive(true);
        pickaxeButton.Select();
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
    public Button GetButtons() { return pickaxeButton; }
}
