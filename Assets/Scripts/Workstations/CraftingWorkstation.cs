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

    public List<WorkshopObjectSO> workshopObjectSOList;

    private void Awake()
    {
        workshopObjectSOList = new List<WorkshopObjectSO>();
    }

    [SerializeField] GameObject CraftingCanvas;
    [SerializeField] private Button pickaxeButton;
    [SerializeField] private CraftingRecipeSO[] craftingPickaxeRecipeSOArray;
    [SerializeField] private CraftingRecipeSO[] craftingAxeRecipeSOArray;
    [SerializeField] private CraftingRecipeSO[] craftingHammerRecipeSOArray;
    public bool isUsing = false;
    private bool recipeCompleted = false;
    private CraftingRecipeSO craftingRecipeSO;
    private CraftingRecipeSO selectedCraftingRecipeSO;

    public enum SelectedRecipe
    {
        Pickaxe,
        Axe,
        Hammer,
    }
    public SelectedRecipe selectedRecipe;

    private void Update()
    {
        Debug.Log(selectedRecipe.ToString());
        Debug.Log(recipeCompleted.ToString());
    }
    public override void Interact(Player player)
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

        if (player.HasWorkshopObject())
        {
            if (TryAddPart(player.GetWorkshopObject().GetWorkshopObjectSO()))
            {
                player.GetWorkshopObject().DestroySelf();
                if (workshopObjectSOList.Count == selectedCraftingRecipeSO.inputSOList.Count)
                {
                    bool contentsMatchesRecipe = true;
                    foreach (WorkshopObjectSO recipeWorkshopObjectSO in workshopObjectSOList)
                    {
                        //Pêtla przez wszystkie czêœci w przepisie 
                        bool partFound = false;
                        foreach (WorkshopObjectSO craftingWorkshopObjectSO in selectedCraftingRecipeSO.inputSOList)
                        {
                            //Pêtla przez wszystkie czêœci
                            if (craftingWorkshopObjectSO == recipeWorkshopObjectSO)
                            {
                                //Czêœci siê zgadzaj¹
                                partFound = true;
                                break;
                            }
                        }
                        if (!partFound)
                        {
                            //Czêœæ z przepisu nie zosta³a znaleziona
                            contentsMatchesRecipe = false;
                        }
                    }
                    if (contentsMatchesRecipe)
                    {
                        recipeCompleted = true;
                        craftingRecipeSO = GetCraftingRecipeSOWithInput(workshopObjectSOList);

                        if (craftingRecipeSO != null && craftingRecipeSO.output != null)
                        {
                            // WorkshopObject.SpawnWorkshopObject(craftingRecipeSO.output, player);

                            
                            Transform handTransform = this.GetWorkshopObjectFollowTransform();
                            if (handTransform != null)
                            {
                                WorkshopObject spawnedObject = WorkshopObject.SpawnWorkshopObject(craftingRecipeSO.output, this);
                                spawnedObject.SetWorkshopObjectParent(this);
                                workshopObjectSOList = new List<WorkshopObjectSO>();
                            }
                        }
                        else
                        {
                            Debug.LogError("Crafting recipe or output is null. Unable to spawn workshop object.");
                        }
                    }
                }
            }
        }
        else if (HasWorkshopObject())
        {
            GetWorkshopObject().SetWorkshopObjectParent(player);
        }

        WorkshopObject workshopObject = this.GetWorkshopObject();
        if (workshopObject != null && workshopObject.TryGetPackage(out PackageWorkshopObject packageWorkshopObject))
        {
            if (packageWorkshopObject.TryAddPart(GetWorkshopObject().GetWorkshopObjectSO()))
            {
                GetWorkshopObject().DestroySelf();
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
        CraftingCanvas.SetActive(true);
        pickaxeButton.Select();
    }

    private void Hide()
    {
        CraftingCanvas.SetActive(false);
    }

    public bool TryAddPart(WorkshopObjectSO workshopObjectSO)
    {
        if (!selectedCraftingRecipeSO.inputSOList.Contains(workshopObjectSO))
        {
            return false;
        }
        //sprawdzenie powtórzeñ
        if (workshopObjectSOList.Contains(workshopObjectSO))
        {
            return false;
        }
        else
        {
            workshopObjectSOList.Add(workshopObjectSO);
            OnPartAdded?.Invoke(this, new OnPartAddedEventArgs
            {
                workshopObjectSO = workshopObjectSO
            });
            return true;
        }
    }

    public List<WorkshopObjectSO> GetWorkshopObjectSOList()
    {
        return workshopObjectSOList;
    }

    private bool HasRecipeWithInput(List<WorkshopObjectSO> inputWorkshopObjectSOArray)
    {
        CraftingRecipeSO craftingRecipeSO = GetCraftingRecipeSOWithInput(inputWorkshopObjectSOArray);
        return craftingRecipeSO != null;
    }
    private WorkshopObjectSO GetOutputForInput(List<WorkshopObjectSO> inputWorkshopObjectSOArray)
    {
        CraftingRecipeSO craftingRecipeSO = GetCraftingRecipeSOWithInput(inputWorkshopObjectSOArray);
        if (craftingRecipeSO != null)
        {
            return craftingRecipeSO.output;
        }
        else
        {
            return null;
        }
    }
    private bool ListsAreEqual(List<WorkshopObjectSO> list1, List<WorkshopObjectSO> list2)
    {
        // Sprawdzenie, czy listy maj¹ tak¹ sam¹ d³ugoœæ
        if (list1.Count != list2.Count)
        {
            return false;
        }
        int checkListCountMax = list1.Count;
        int checkListCount = 0;
        foreach (WorkshopObjectSO item in list1)
        {
            foreach (WorkshopObjectSO item2 in list2)
            {
                if (item == item2)
                {
                    checkListCount++;
                    if (checkListCount == checkListCountMax)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }
   
    private CraftingRecipeSO GetCraftingRecipeSOWithInput(List<WorkshopObjectSO> inputWorkshopObjectSOArray)
    {
        switch (selectedRecipe)
        {
            case SelectedRecipe.Pickaxe:
                foreach (CraftingRecipeSO craftingRecipeSO in craftingPickaxeRecipeSOArray)
                {
                    if (ListsAreEqual(craftingRecipeSO.inputSOList, inputWorkshopObjectSOArray))
                    {
                        return craftingRecipeSO;
                    }
                }
                return null;
            case SelectedRecipe.Axe:
                foreach (CraftingRecipeSO craftingRecipeSO in craftingAxeRecipeSOArray)
                {
                    if (ListsAreEqual(craftingRecipeSO.inputSOList, inputWorkshopObjectSOArray))
                    {
                        return craftingRecipeSO;
                    }
                }
                return null;
            case SelectedRecipe.Hammer:
                foreach (CraftingRecipeSO craftingRecipeSO in craftingHammerRecipeSOArray)
                {
                    if (ListsAreEqual(craftingRecipeSO.inputSOList, inputWorkshopObjectSOArray))
                    {
                        return craftingRecipeSO;
                    }
                }
                return null;
        }
        return null;
    }
}
