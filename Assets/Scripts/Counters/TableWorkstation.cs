using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableWorkstation : BaseWorkstation
{
    [SerializeField] private WorkshopObjectSO workshopObjectSO;

    public override void Interact(Player player)
    {
        if (!HasWorkshopObject())
        {
            if (player.HasWorkshopObject()) {
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
                if (player.GetWorkshopObject().TryGetPlate(out PlateWorkshopObject plateWorkshopObject)) 
                {
                    if (plateWorkshopObject.TryAddIngredient(GetWorkshopObject().GetWorkshopObjectSO()))
                    {
                        GetWorkshopObject().DestroySelf();
                    }
                }
                else
                {
                    if (GetWorkshopObject().TryGetPlate(out plateWorkshopObject))
                    {
                        if (plateWorkshopObject.TryAddIngredient(player.GetWorkshopObject().GetWorkshopObjectSO()))
                        {
                            player.GetWorkshopObject().DestroySelf();
                        }
                    }
                }
            }
        }
    }
}
