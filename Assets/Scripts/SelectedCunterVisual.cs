using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedCunterVisual : MonoBehaviour
{
    [SerializeField] private ClearCunter clearCunter;
    [SerializeField] private GameObject visualgameObject;
    private void Start()
    {
        Player.Instance.OnselectedCunterChanged += Player_OnselectedCunterChanged;
    }

    private void Player_OnselectedCunterChanged(object sender, Player.OnSelectedCounterChangedEventArgs e)
    {
        if(e.selectedCunter==clearCunter) {
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
