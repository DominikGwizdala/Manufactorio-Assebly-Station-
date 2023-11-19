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

    private void Awake()
    {
        workshopObjectSOList = new List<WorkshopObjectSO>();
    }

    [SerializeField] GameObject CraftingCanvas;
    [SerializeField] private Button pickaxeButton;
    [SerializeField] private CraftingRecipeSO[] craftingPickaxeRecipeSOArray;
    [SerializeField] private CraftingRecipeSO[] craftingAxeRecipeSOArray;
    [SerializeField] private CraftingRecipeSO[] craftingHoeRecipeSOArray;
    public bool isUsing = false;
    private bool recipeCompleted = false;
    private CraftingRecipeSO craftingRecipeSO;

    public enum SelectedRecipe
    {
        Pickaxe,
        Axe,
        Hoe,
    }
    public SelectedRecipe selectedRecipe;

    private void Update()
    {
        Debug.Log(selectedRecipe.ToString());
        Debug.Log(recipeCompleted.ToString());
    }
    public override void Interact(Player player)
    {
        if (player.HasWorkshopObject())
        {
            if (TryAddPart(player.GetWorkshopObject().GetWorkshopObjectSO()))
            {
                player.GetWorkshopObject().DestroySelf();
                if (workshopObjectSOList.Count == craftingPickaxeRecipeSOArray[0].inputSOList.Count)
                {
                    bool contentsMatchesRecipe = true;
                    foreach (WorkshopObjectSO recipeWorkshopObjectSO in workshopObjectSOList)
                    {
                        //Pêtla przez wszystkie czêœci w przepisie 
                        bool partFound = false;
                        foreach (WorkshopObjectSO craftingWorkshopObjectSO in craftingPickaxeRecipeSOArray[0].inputSOList)
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
        if (!craftingPickaxeRecipeSOArray[0].inputSOList.Contains(workshopObjectSO))
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
        // Sprawdzenie, czy elementy list s¹ identyczne
        //list1.OrderBy(t => t);
        //list2.OrderBy(t => t);
        //return list1.SequenceEqual(list2);
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
            case SelectedRecipe.Hoe:
                foreach (CraftingRecipeSO craftingRecipeSO in craftingHoeRecipeSOArray)
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
