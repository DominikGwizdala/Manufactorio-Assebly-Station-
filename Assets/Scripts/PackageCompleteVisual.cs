using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PackageCompleteVisual;

public class PackageCompleteVisual : MonoBehaviour
{
    [Serializable] public struct WorkshopObjectSO_GameObject
    {
        public WorkshopObjectSO workshopObjectSO;
        public GameObject gameObject;
    }

    [SerializeField] private PackageWorkshopObject packageWorkshopObject;
    [SerializeField] private List<WorkshopObjectSO_GameObject> workshopObjectSOGameObjectList;

    private void Start()
    {
        packageWorkshopObject.OnPartAdded += PackageWorkshopObject_OnPartAdded;

        foreach (WorkshopObjectSO_GameObject workshopObjectSOGameObject in workshopObjectSOGameObjectList)
        {
            workshopObjectSOGameObject.gameObject.SetActive(false);
        }
    }

    private void PackageWorkshopObject_OnPartAdded(object sender, PackageWorkshopObject.OnPartAddedEventArgs e)
    {
        foreach (WorkshopObjectSO_GameObject workshopObjectSOGameObject in workshopObjectSOGameObjectList)
        {
            if (workshopObjectSOGameObject.workshopObjectSO == e.workshopObjectSO)
            {
                workshopObjectSOGameObject.gameObject.SetActive(true);
            }
        }
    }
}
