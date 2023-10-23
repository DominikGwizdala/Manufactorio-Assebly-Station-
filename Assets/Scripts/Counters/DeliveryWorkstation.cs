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
            if (player.GetWorkshopObject().TryGetPlate(out PlateWorkshopObject plateWorkshopObject))
            {
                DeliveryManager.Instance.DeliverRecipe(plateWorkshopObject);
                player.GetWorkshopObject().DestroySelf();
            }
        }
    }
}
