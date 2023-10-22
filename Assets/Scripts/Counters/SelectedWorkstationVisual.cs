using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedWorkstationVisual : MonoBehaviour
{
    [SerializeField] private BaseWorkstation baseWorkstation;
    [SerializeField] private GameObject[] visualGameObjectArray;
    private void Start()
    {
        Player.Instance.OnSelectedWorkstationChanged += Player_OnSelectedWorkstationChanged;
    }

    private void Player_OnSelectedWorkstationChanged(object sender, Player.OnSelectedWorkstationChangedEventArgs e)
    {
        if (e.selectedWorkstation == baseWorkstation) 
        {
            Show();
        }
        else
        {
            Hide();
        }
    }
    private void Show() {
        foreach (GameObject visualGameObject in visualGameObjectArray)
        {
            visualGameObject.SetActive(true);
        }
    }

    private void Hide() {
        foreach (GameObject visualGameObject in visualGameObjectArray)
        {
            visualGameObject.SetActive(false);
        }
    }
}
