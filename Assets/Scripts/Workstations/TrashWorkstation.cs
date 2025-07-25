using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashWorkstation : BaseWorkstation
{
    public static EventHandler OnAnyObjectTrashed;
    new public static void ResetStaticData()
    {
        OnAnyObjectTrashed = null;
    }
    public override void Interact(Player player)
    {
        if (player.HasWorkshopObject())
        {
            player.GetWorkshopObject().DestroySelf();
            OnAnyObjectTrashed?.Invoke(this, EventArgs.Empty);
        }
    }
}
