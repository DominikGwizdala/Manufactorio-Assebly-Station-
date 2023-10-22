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
        bool showVisual = e.state == FurnaceWorkstation.State.Frying || e.state == FurnaceWorkstation.State.Fried;
        furnaceOnGameObject.SetActive(showVisual);
        particlesGameObject.SetActive(showVisual);
    }
}
