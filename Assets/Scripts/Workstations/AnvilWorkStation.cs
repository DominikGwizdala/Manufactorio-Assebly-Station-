using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.Runtime.CompilerServices.RuntimeHelpers;
using UnityEngine.UI;

public class AnvilWorkstation : BaseWorkstation, IHasProgress
{
    public static event EventHandler OnAnyForge;
  new public static void ResetStaticData()
    {
        OnAnyForge = null;
    }

    [SerializeField] GameObject AnvilCanvas;
    [SerializeField] private Button firstButton;
    [SerializeField] private ForgingRecipeSO[] forgingPickaxeRecipeSOArray;
    [SerializeField] private ForgingRecipeSO[] forgingAxeRecipeSOArray;
    [SerializeField] private ForgingRecipeSO[] forgingHammerRecipeSOArray;
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    public event EventHandler OnForge;
    public bool isUsing = false;
    public enum SelectedRecipe
    {
        Pickaxe,
        Axe,
        Hammer,
    }
    public SelectedRecipe selectedRecipe;

    public int forgingProgress;

    public override void Interact(Player player)
    {
        if (!HasWorkshopObject())
        {
            if (player.HasWorkshopObject())
            {
                player.GetWorkshopObject().SetWorkshopObjectParent(this);
                forgingProgress = 0;
                ForgingRecipeSO forgingRecipeSO = GetForgingRecipeSOWithInput(GetWorkshopObject().GetWorkshopObjectSO());
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                {
                    progressNormalized = (float)forgingProgress / forgingRecipeSO.forgingProgressMax
                });
            }
        }
        else
        {
            if (!player.HasWorkshopObject())
            {
                GetWorkshopObject().transform.rotation = player.transform.rotation;
                GetWorkshopObject().SetWorkshopObjectParent(player);

                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                {
                    progressNormalized = 0f
                });
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
            }
        }
    }

    public override void InteractAlternate(Player player)
    {
        if (HasWorkshopObject() && HasRecipeWithInput(GetWorkshopObject().GetWorkshopObjectSO()))
        {
            forgingProgress++;
            OnForge?.Invoke(this, EventArgs.Empty);
            //Debug.Log(OnAnyForge.GetInvocationList().Length);
            OnAnyForge?.Invoke(this, EventArgs.Empty);

            ForgingRecipeSO forgingRecipeSO = GetForgingRecipeSOWithInput(GetWorkshopObject().GetWorkshopObjectSO());
            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
            {
                progressNormalized = (float)forgingProgress / forgingRecipeSO.forgingProgressMax
            });
            if (forgingProgress >= forgingRecipeSO.forgingProgressMax)
            {
                WorkshopObjectSO outputWorkshopObjectSO = GetOutputForInput(GetWorkshopObject().GetWorkshopObjectSO());
                GetWorkshopObject().DestroySelf();

                WorkshopObject.SpawnWorkshopObject(outputWorkshopObjectSO, this);
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
        firstButton.Select();
        AnvilCanvas.SetActive(true);
    }

    private void Hide()
    {
        AnvilCanvas.SetActive(false);
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
            case SelectedRecipe.Hammer:
                foreach (ForgingRecipeSO forgingRecipeSO in forgingHammerRecipeSOArray)
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

