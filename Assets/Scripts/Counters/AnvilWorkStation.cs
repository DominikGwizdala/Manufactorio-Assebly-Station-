using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.Runtime.CompilerServices.RuntimeHelpers;
using UnityEngine.UI;

public class AnvilWorkstation : BaseWorkstation, IHasProgress
{
    [SerializeField] GameObject AnvilCanvas;
    [SerializeField] private Button pickaxeButton;
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    public event EventHandler OnCut;
    public int forgeProgress;
    public bool isUsing = false;

    public override void Interact(Player player)
    {
        if (!HasWorkshopObject())
        {
            if (player.HasWorkshopObject())
            {
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

    public override void InteractAnvil(Player player)
    {
        if (GameManager.Instance.isGamePaused == false)
        {
            if (isUsing == false)
            {
                Show();
                isUsing = true;
                GameManager.Instance.ToggleUsingAnvil();
            }
            else if (isUsing == true)
            {
                Hide();
                isUsing = false;
                GameManager.Instance.ToggleUsingAnvil();
            }
        }
    }
    public void Show()
    {
        AnvilCanvas.SetActive(true);
        pickaxeButton.Select();
    }

    private void Hide()
    {
        AnvilCanvas.SetActive(false);
    }
}

