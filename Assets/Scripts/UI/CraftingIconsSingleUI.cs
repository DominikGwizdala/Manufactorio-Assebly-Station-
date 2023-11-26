using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingIconsSingleUI : MonoBehaviour
{
    [SerializeField] private Image image;

    public void SetWorkshopObjectSO(WorkshopObjectSO workshopObjectSO)
    {
        image.sprite = workshopObjectSO.sprite;
    }
}
