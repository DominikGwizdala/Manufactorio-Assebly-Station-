using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurnaceBurnWarningUI : MonoBehaviour
{
    [SerializeField] private FurnaceWorkstation furnaceWorkstation;

    private void Start()
    {
        furnaceWorkstation.OnProgressChanged += FurnaceWorkstation_OnProgressChanged;

        Hide();
    }

    private void FurnaceWorkstation_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
    {
        float burnShowProgressAmount = .5f;
        bool show = furnaceWorkstation.IsFried() && e.progressNormalized >= burnShowProgressAmount;

        if (show)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
