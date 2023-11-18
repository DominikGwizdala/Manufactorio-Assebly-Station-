using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingWorkstation : BaseWorkstation
{
    [SerializeField] GameObject CraftingCanvas;
    [SerializeField] private Button pickaxeButton;
    [SerializeField] private ForgingRecipeSO[] forgingPickaxeRecipeSOArray;
    [SerializeField] private ForgingRecipeSO[] forgingAxeRecipeSOArray;
    [SerializeField] private ForgingRecipeSO[] forgingHoeRecipeSOArray;
    public bool isUsing = false;
    public enum SelectedRecipe
    {
        Pickaxe,
        Axe,
        Hoe,
    }
    public SelectedRecipe selectedRecipe;

    public override void Interact(Player player)
    {
        if (!HasWorkshopObject())
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

    private bool HasRecipeWithInput(WorkshopObjectSO inputWorkshopObjectSO)
    {
        ForgingRecipeSO forgingRecipeSO = GetForgingRecipeSOWithInput(inputWorkshopObjectSO);
        return forgingRecipeSO != null;
    }
    private WorkshopObjectSO GetOutputForInput(WorkshopObjectSO inputWorkshopObjectSO)
    {
        ForgingRecipeSO forgingRecipeSO = GetForgingRecipeSOWithInput(inputWorkshopObjectSO);
        if (forgingRecipeSO != null)
        {
            return forgingRecipeSO.output;
        }
        else
        {
            return null;
        }
    }
    private ForgingRecipeSO GetForgingRecipeSOWithInput(WorkshopObjectSO inputWorkshopObjectSO)
    {
        switch (selectedRecipe)
        {
            case SelectedRecipe.Pickaxe:
                foreach (ForgingRecipeSO forgingRecipeSO in forgingPickaxeRecipeSOArray)
                {
                    if (forgingRecipeSO.input == inputWorkshopObjectSO)
                    {
                        return forgingRecipeSO;
                    }
                }
                return null;
            case SelectedRecipe.Axe:
                foreach (ForgingRecipeSO forgingRecipeSO in forgingAxeRecipeSOArray)
                {
                    if (forgingRecipeSO.input == inputWorkshopObjectSO)
                    {
                        return forgingRecipeSO;
                    }
                }
                return null;
            case SelectedRecipe.Hoe:
                foreach (ForgingRecipeSO forgingRecipeSO in forgingHoeRecipeSOArray)
                {
                    if (forgingRecipeSO.input == inputWorkshopObjectSO)
                    {
                        return forgingRecipeSO;
                    }
                }
                return null;
        }
        return null;
    }
}
