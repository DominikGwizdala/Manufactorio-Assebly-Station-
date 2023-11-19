using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurnaceWorkstationVisual : MonoBehaviour
{
    [SerializeField] private FurnaceWorkstation furnaceWorkstation;
    [SerializeField] private GameObject furnaceOnGameObject;
    [SerializeField] private GameObject particlesGameObject;

    private void Start()
    {
        furnaceWorkstation.OnStateChanged += FurnaceWorkstation_OnStateChanged;
    }

    private void FurnaceWorkstation_OnStateChanged(object sender, FurnaceWorkstation.OnStateChangedEventArgs e)
    {
        bool showVisual = e.state == FurnaceWorkstation.State.Smelting || e.state == FurnaceWorkstation.State.Smelted;
        furnaceOnGameObject.SetActive(showVisual);
        particlesGameObject.SetActive(showVisual);
    }
}
