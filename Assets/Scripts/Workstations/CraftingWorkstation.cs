using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingWorkstation : BaseWorkstation
{
    public event EventHandler<OnPartAddedEventArgs> OnPartAdded;
    public class OnPartAddedEventArgs : EventArgs
    {
        public WorkshopObjectSO workshopObjectSO;
    }

    [SerializeField] private List<WorkshopObjectSO> validWorkshopObjectSOList;

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
        /*if (!HasWorkshopObject())
        {
            if (player.HasWorkshopObject())
            {
                player.GetWorkshopObject().SetWorkshopObjectParent(this);
            }
        }
        else
        {
            if (!player.HasWorkshopObject())
            {
                GetWorkshopObject().SetWorkshopObjectParent(player);
            }
            else
            {
                if (player.GetWorkshopObject().TryGetPackage(out PackageWorkshopObject packageWorkshopObject))
                {
                    if (packageWorkshopObject.TryAddPart(GetWorkshopObject().GetWorkshopObjectSO()))
                    {
                        GetWorkshopObject().DestroySelf();
                    }
                }
                else
                {
                    if (GetWorkshopObject().TryGetPackage(out packageWorkshopObject))
                    {
                        if (packageWorkshopObject.TryAddPart(player.GetWorkshopObject().GetWorkshopObjectSO()))
                        {
                            player.GetWorkshopObject().DestroySelf();
                        }
                    }
                }
            }
        }*/

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
                        /*WorkshopObjectSO outputWorkshopObjectSO = GetOutputForInput(workshopObjectSOList);
                        GetWorkshopObject().DestroySelf();

                        WorkshopObject.SpawnWorkshopObject(outputWorkshopObjectSO, this);*/
                        craftingRecipeSO = GetCraftingRecipeSOWithInput(workshopObjectSOList);
                        WorkshopObject.SpawnWorkshopObject(craftingRecipeSO.output, player);
                    }
                }
            }
        }

        if (HasWorkshopObject())
        {
            GetWorkshopObject().SetWorkshopObjectParent(player);
        }

        if (player.GetWorkshopObject().TryGetPackage(out PackageWorkshopObject packageWorkshopObject))
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
    private CraftingRecipeSO GetCraftingRecipeSOWithInput(List<WorkshopObjectSO> inputWorkshopObjectSOArray)
    {
        switch (selectedRecipe)
        {
            case SelectedRecipe.Pickaxe:
                foreach (CraftingRecipeSO craftingRecipeSO in craftingPickaxeRecipeSOArray)
                {
                    if (craftingRecipeSO.inputSOList == inputWorkshopObjectSOArray)
                    {
                        return craftingRecipeSO;
                    }
                }
                return null;
            case SelectedRecipe.Axe:
                foreach (CraftingRecipeSO craftingRecipeSO in craftingAxeRecipeSOArray)
                {
                    if (craftingRecipeSO.inputSOList == inputWorkshopObjectSOArray)
                    {
                        return craftingRecipeSO;
                    }
                }
                return null;
            case SelectedRecipe.Hoe:
                foreach (CraftingRecipeSO craftingRecipeSO in craftingHoeRecipeSOArray)
                {
                    if (craftingRecipeSO.inputSOList == inputWorkshopObjectSOArray)
                    {
                        return craftingRecipeSO;
                    }
                }
                return null;
        }
        return null;
    }
}
