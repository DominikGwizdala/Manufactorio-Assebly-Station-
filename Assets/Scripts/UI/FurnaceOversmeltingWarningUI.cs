using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurnaceOversmeltingWarningUI : MonoBehaviour
{
    [SerializeField] private FurnaceWorkstation furnaceWorkstation;

    private void Start()
    {
        furnaceWorkstation.OnProgressChanged += FurnaceWorkstation_OnProgressChanged;

        Hide();
    }

    private void FurnaceWorkstation_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
    {
        float oversmeltingShowProgressAmount = .5f;
        bool show = furnaceWorkstation.IsSmelted() && e.progressNormalized >= oversmeltingShowProgressAmount;

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
