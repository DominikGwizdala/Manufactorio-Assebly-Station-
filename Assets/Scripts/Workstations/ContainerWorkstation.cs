using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerWorkstation : BaseWorkstation
{
    [SerializeField] private WorkshopObjectSO workshopObjectSO;

    public event EventHandler OnPlayerGrabbedObject;

    public override void Interact(Player player)
    {
        if (!player.HasWorkshopObject())
        {
            WorkshopObject.SpawnWorkshopObject(workshopObjectSO, player);

            OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);
        }
    }
}
