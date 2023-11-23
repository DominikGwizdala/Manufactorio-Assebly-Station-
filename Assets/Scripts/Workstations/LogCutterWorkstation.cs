using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LogCutterWorkstation : BaseWorkstation, IHasProgress
{
    public static event EventHandler OnAnyCut;
  new public static void ResetStaticData()
    {
        OnAnyCut = null;
    }

    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    public event EventHandler OnCut;
    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOArray;

    public int cuttingProgress;

    public override void Interact(Player player)
    {
        if (!HasWorkshopObject())
        {
            if (player.HasWorkshopObject())
            {
                player.GetWorkshopObject().SetWorkshopObjectParent(this);
                cuttingProgress = 0;
                CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetWorkshopObject().GetWorkshopObjectSO());
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                {
                    progressNormalized = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax
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
            cuttingProgress++;
            OnCut?.Invoke(this, EventArgs.Empty);
            Debug.Log(OnAnyCut.GetInvocationList().Length);
            OnAnyCut?.Invoke(this, EventArgs.Empty);

            CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetWorkshopObject().GetWorkshopObjectSO());
            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
            {
                progressNormalized = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax
            });
            if (cuttingProgress >= cuttingRecipeSO.cuttingProgressMax)
            {
                WorkshopObjectSO outputWorkshopObjectSO = GetOutputForInput(GetWorkshopObject().GetWorkshopObjectSO());
                GetWorkshopObject().DestroySelf();

                WorkshopObject.SpawnWorkshopObject(outputWorkshopObjectSO, this);
            }
        }
    }
    private bool HasRecipeWithInput(WorkshopObjectSO inputWorkshopObjectSO)
    {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputWorkshopObjectSO);
        return cuttingRecipeSO != null;
    }
    private WorkshopObjectSO GetOutputForInput(WorkshopObjectSO inputWorkshopObjectSO)
    {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputWorkshopObjectSO);
        if(cuttingRecipeSO != null)
        {
            return cuttingRecipeSO.output;
        }
        else
        {
            return null;
        }
    }
    private CuttingRecipeSO GetCuttingRecipeSOWithInput(WorkshopObjectSO inputWorkshopObjectSO) 
    {
        foreach (CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSOArray)
        {

            if (cuttingRecipeSO.input == inputWorkshopObjectSO)
            {
                return cuttingRecipeSO;
            }
        }
        return null;
    }
}
