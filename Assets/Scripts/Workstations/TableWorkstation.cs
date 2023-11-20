using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableWorkstation : BaseWorkstation
{
    //[SerializeField] private WorkshopObjectSO workshopObjectSO;

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
                GetWorkshopObject().transform.rotation = GetWorkshopObject().transform.parent.rotation;
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
}
