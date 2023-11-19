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
                        //P�tla przez wszystkie cz�ci w przepisie 
                        bool partFound = false;
                        foreach (WorkshopObjectSO craftingWorkshopObjectSO in craftingPickaxeRecipeSOArray[0].inputSOList)
                        {
                            //P�tla przez wszystkie cz�ci
                            if (craftingWorkshopObjectSO == recipeWorkshopObjectSO)
                            {
                                //Cz�ci si� zgadzaj�
                                partFound = true;
                                break;
                            }
                        }
                        if (!partFound)
                        {
                            //Cz�� z przepisu nie zosta�a znaleziona
                            contentsMatchesRecipe = false;
                        }
                    }
                    if (contentsMatchesRecipe)
                    {
                        recipeCompleted = true;
                        /*WorkshopObjectSO outputWorkshopObjectSO = GetOutputForInput(workshopObjectSOList);
                        GetWorkshopObject().DestroySelf();

                        WorkshopObject.SpawnWorkshopObject(outputWorkshopObjectSO, this);*/
                      
                                
                            craftingRecipeSO = GetCraftingRecipeSOWithInput(workshopObjectSOList);

                        if (craftingRecipeSO != null && craftingRecipeSO.output != null)
                        {
                            WorkshopObject.SpawnWorkshopObject(craftingRecipeSO.output, player);
                        }
                        else
                        {
                            Debug.LogError("Crafting recipe or output is null. Unable to spawn workshop object.");
                        }
                    }
                }
            }
        }

        if (HasWorkshopObject())
        {
            GetWorkshopObject().SetWorkshopObjectParent(player);
        }

        WorkshopObject workshopObject = player.GetWorkshopObject();
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
        //sprawdzenie powt�rze�
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
        // Sprawdzenie, czy listy maj� tak� sam� d�ugo��
        if (list1.Count != list2.Count)
        {
            return false;
        }

        // Sprawdzenie, czy elementy list s� identyczne
        return list1.SequenceEqual(list2);
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
