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

    [SerializeField] GameObject anvilCanvas;
    [SerializeField] private ForgingRecipeSO[] forgingRecipeSOArray;
    [SerializeField] private AnvilRecipeUI anvilRecipeUI;
    public ForgingRecipeSO selectedForgingRecipeSO;
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    public event EventHandler OnForge;
    private bool isUsing = false;
    private bool firstShow = true;

    public int forgingProgress;

    private void Awake()
    {
        selectedForgingRecipeSO = forgingRecipeSOArray[0];
        anvilCanvas.SetActive(false);
    }

    public override void Interact(Player player)
    {
        if (!HasWorkshopObject())
        {
            if (player.HasWorkshopObject())
            {
                player.GetWorkshopObject().SetWorkshopObjectParent(this);
                forgingProgress = 0;
                ForgingRecipeSO forgingRecipeSO = selectedForgingRecipeSO;
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
        if (HasWorkshopObject() && selectedForgingRecipeSO != null)
        {
            forgingProgress++;
            OnForge?.Invoke(this, EventArgs.Empty);
            //Debug.Log(OnAnyForge.GetInvocationList().Length);
            OnAnyForge?.Invoke(this, EventArgs.Empty);

            ForgingRecipeSO forgingRecipeSO = selectedForgingRecipeSO;
            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
            {
                progressNormalized = (float)forgingProgress / forgingRecipeSO.forgingProgressMax
            });
            if (forgingProgress >= forgingRecipeSO.forgingProgressMax)
            {
                WorkshopObjectSO outputWorkshopObjectSO = selectedForgingRecipeSO.output;
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
                if (firstShow == false)
                {
                    anvilRecipeUI.SelectFirstButton();
                }
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
        anvilCanvas.SetActive(true);
        firstShow = false;
    }

    private void Hide()
    {
        anvilCanvas.SetActive(false);
    }

    public void SelectRecipeActions()
    {
        Hide();
        isUsing = false;
    }

    public ForgingRecipeSO[] GetForgingRecipeSOArray()
    {
        return forgingRecipeSOArray;
    }
}
