using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour
{
    [SerializeField] private ClearCounter clearCounter;
    [SerializeField] private GameObject visualgameObject;
    private void Start()
    {
        Player.Instance.OnselectedCounterChanged += Player_OnselectedCounterChanged;
    }

    private void Player_OnselectedCounterChanged(object sender, Player.OnSelectedCounterChangedEventArgs e)
    {
        if(e.selectedCounter==clearCounter) {
            Show();
        }else{
            Hide();
        }
    }
    private void Show() {
        visualgameObject.SetActive(true);
    }

    private void Hide() {
        visualgameObject.SetActive(false);
    }
}
