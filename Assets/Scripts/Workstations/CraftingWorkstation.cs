using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CraftingWorkstation : BaseWorkstation
{
    public event EventHandler<OnPartAddedEventArgs> OnPartAdded;
    public class OnPartAddedEventArgs : EventArgs
    {
        public WorkshopObjectSO workshopObjectSO;
    }

    private List<WorkshopObjectSO> workshopObjectSOList;
    private List<WorkshopObjectSO> toRemoveObjectSOList;

    private void Awake()
    {
        workshopObjectSOList = new List<WorkshopObjectSO>();
        toRemoveObjectSOList = new List<WorkshopObjectSO>();
        foreach (WorkshopObjectSO item in craftingPickaxeRecipeSOArray[0].inputSOList)
        {
            toRemoveObjectSOList.Add(item);
        }
        selectedCraftingRecipeSO = craftingPickaxeRecipeSOArray[0];
        selectedButton.Select();
    }

    [SerializeField] GameObject craftingCanvas;
    [SerializeField] private CraftingRecipeSO[] craftingPickaxeRecipeSOArray;
    [SerializeField] private CraftingRecipeSO[] craftingAxeRecipeSOArray;
    [SerializeField] private CraftingRecipeSO[] craftingHammerRecipeSOArray;
    [SerializeField] private Button selectedButton;
    public bool isUsing = false;
    private CraftingRecipeSO craftingRecipeSO;
    private CraftingRecipeSO selectedCraftingRecipeSO;
    public enum SelectedRecipe
    {
        Pickaxe,
        Axe,
        Hammer,
    }
    public SelectedRecipe selectedRecipe;

    public override void Interact(Player player)
    {
        if (player.HasWorkshopObject())
        {
            if (TryAddPart(player.GetWorkshopObject().GetWorkshopObjectSO()))
            {
                player.GetWorkshopObject().DestroySelf();
                if (workshopObjectSOList.Count == selectedCraftingRecipeSO.inputSOList.Count)
                {
                    craftingRecipeSO = selectedCraftingRecipeSO;

                    if (craftingRecipeSO != null && craftingRecipeSO.output != null)
                    {
                        Transform handTransform = GetWorkshopObjectFollowTransform();
                        if (handTransform != null)
                        {
                            WorkshopObject spawnedObject = WorkshopObject.SpawnWorkshopObject(craftingRecipeSO.output, this);
                            spawnedObject.SetWorkshopObjectParent(this);
                            workshopObjectSOList = new List<WorkshopObjectSO>();
                            ToRemoveNewList();
                        }
                    }
                }
            }
        }
        else if (HasWorkshopObject())
        {
            GetWorkshopObject().SetWorkshopObjectParent(player);
        }

        WorkshopObject workshopObject = GetWorkshopObject();
        if (workshopObject != null && player.HasWorkshopObject())
        {
            if (player.GetWorkshopObject().TryGetPackage(out PackageWorkshopObject packageWorkshopObject))
            {
                if (packageWorkshopObject.TryAddPart(GetWorkshopObject().GetWorkshopObjectSO()))
                {
                    GetWorkshopObject().DestroySelf();
                }
            }
        }
    }

    public override void InteractCrafting(Player player)
    {
        if (GameManager.Instance.isGamePaused == false)
        {
            if (isUsing == false)
            {
                Show();
                isUsing = true;
                GameManager.Instance.ToggleUsingCrafting();
            }
            else if (isUsing == true)
            {
                Hide();
                isUsing = false;
                GameManager.Instance.ToggleUsingCrafting();
            }
        }
    }
    public void Show()
    {
        craftingCanvas.SetActive(true);
    }

    private void Hide()
    {
        craftingCanvas.SetActive(false);
    }

    public bool TryAddPart(WorkshopObjectSO workshopObjectSO)
    {
        if (!toRemoveObjectSOList.Contains(workshopObjectSO))
        {
            return false;
        }
        else
        {
            workshopObjectSOList.Add(workshopObjectSO);
            toRemoveObjectSOList.Remove(workshopObjectSO);
            OnPartAdded?.Invoke(this, new OnPartAddedEventArgs
            {
                workshopObjectSO = workshopObjectSO
            });
            return true;
        }
    }

    public void SelectRecipe()
    {
        switch (selectedRecipe)
        {
            case SelectedRecipe.Pickaxe:
                selectedCraftingRecipeSO = craftingPickaxeRecipeSOArray[0];
                break;
            case SelectedRecipe.Axe:
                selectedCraftingRecipeSO = craftingAxeRecipeSOArray[0];
                break;
            case SelectedRecipe.Hammer:
                selectedCraftingRecipeSO = craftingHammerRecipeSOArray[0];
                break;
        }
        workshopObjectSOList = new List<WorkshopObjectSO>();
        ToRemoveNewList();
    }

    private void ToRemoveNewList()
    {
        toRemoveObjectSOList = new List<WorkshopObjectSO>();
        foreach (WorkshopObjectSO item in selectedCraftingRecipeSO.inputSOList)
        {
            toRemoveObjectSOList.Add(item);
        }
    }
}
