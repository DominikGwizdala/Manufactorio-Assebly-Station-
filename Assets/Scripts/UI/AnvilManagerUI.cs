using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class AnvilManagerUI : MonoBehaviour
{
    public static AnvilManagerUI Instance { get; private set; }

    [SerializeField] private Button pickaxeButton;
    [SerializeField] private Button axeButton;
    [SerializeField] private Button hammerButton;
    [SerializeField] private AnvilWorkstation anvilWorkstation;

    private void Awake()
    {
        Instance = this;

        pickaxeButton.onClick.AddListener(() => {
            Debug.Log("Wybrano kilof");
            anvilWorkstation.selectedRecipe = AnvilWorkstation.SelectedRecipe.Pickaxe;
            OnClickActions();
        });
        axeButton.onClick.AddListener(() => {
            Debug.Log("Wybrano siekiere");
            anvilWorkstation.selectedRecipe = AnvilWorkstation.SelectedRecipe.Axe;
            OnClickActions();
        });
        hammerButton.onClick.AddListener(() => {
            Debug.Log("Wybrano motyke");
            anvilWorkstation.selectedRecipe = AnvilWorkstation.SelectedRecipe.Hammer;
            OnClickActions();
        });
    }

    private void OnClickActions()
    {
        anvilWorkstation.SelectRecipe();
        GameManager.Instance.ToggleUsingCrafting();
    }
}
