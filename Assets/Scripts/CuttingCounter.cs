using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounter : BaseCounter
{
    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSoArray;

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            if (player.HasKitchenObject())
            {
                player.GetKitchenObject().SetKitchenObjectParent(this);
            }
        }
        else
        {
            if (!player.HasKitchenObject())
            {
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }
    public override void InteractAlternate(Player player)
    {
        if(HasKitchenObject())
        {
            KitchenObjectSO outputKitchenObjectSo =GetOutputForInput(GetKitchenObject().GetKitchenObjectSO());
            GetKitchenObject().DestroySelf();
            
            KitchenObject.SpawnKitchenObject(outputKitchenObjectSo, this);

        }
    }
    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSoArray) { 
        
            if(cuttingRecipeSO.input == inputKitchenObjectSO)
            {
                return cuttingRecipeSO.output;
            }            
        }
        return null;
    }
}
