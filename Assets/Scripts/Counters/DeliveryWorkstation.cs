using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryWorkstation : BaseWorkstation
{
    public static DeliveryWorkstation Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public override void Interact(Player player)
    {
        if (player.HasWorkshopObject()) 
        {
            if (player.GetWorkshopObject().TryGetPackage(out PackageWorkshopObject packageWorkshopObject))
            {
                DeliveryManager.Instance.DeliverRecipe(packageWorkshopObject);
                player.GetWorkshopObject().DestroySelf();
            }
        }
    }
}
