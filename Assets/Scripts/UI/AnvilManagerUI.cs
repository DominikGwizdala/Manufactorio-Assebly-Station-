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
    [SerializeField] private Button hoeButton;
    [SerializeField] private AnvilWorkstation anvilWorkstation;

    private void Awake()
    {
        Instance = this;

        pickaxeButton.onClick.AddListener(() => {
            Debug.Log("Wybrano kilof");
            anvilWorkstation.selectedRecipe = AnvilWorkstation.SelectedRecipe.Pickaxe;
            anvilWorkstation.isUsing = false;
            Hide();
            GameManager.Instance.ToggleUsingAnvil();
        });
        axeButton.onClick.AddListener(() => {
            Debug.Log("Wybrano siekiere");
            anvilWorkstation.selectedRecipe = AnvilWorkstation.SelectedRecipe.Axe;
            anvilWorkstation.isUsing = false;
            Hide();
            GameManager.Instance.ToggleUsingAnvil();
        });
        hoeButton.onClick.AddListener(() => {
            Debug.Log("Wybrano motyke");
            anvilWorkstation.selectedRecipe = AnvilWorkstation.SelectedRecipe.Hoe;
            anvilWorkstation.isUsing = false;
            Hide();
            GameManager.Instance.ToggleUsingAnvil();
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
