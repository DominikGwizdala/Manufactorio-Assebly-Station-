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

    public override void InteractAnvil(Player player)
    {
        if (isUsing == false)
        {
            Show();
            isUsing = true;
        }
        else if (isUsing == true)
        {
            Hide();
            isUsing = false;
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

