using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesWorkstationVisual : MonoBehaviour
{
    [SerializeField] private PlatesWorkstation platesWorkstation;
    [SerializeField] private Transform workstationTopPoint;
    [SerializeField] private Transform plateVisualPrefab;

    private List<GameObject> plateVisualGameObjectList;

    private void Awake()
    {
        plateVisualGameObjectList = new List<GameObject>();
    }

    private void Start()
    {
        platesWorkstation.OnPlateSpawned += PlatesWorkstation_OnPlateSpawned;
        platesWorkstation.OnPlateRemoved += PlatesWorkstation_OnPlateRemoved;
    }

    private void PlatesWorkstation_OnPlateRemoved(object sender, System.EventArgs e)
    {
        GameObject plateGameObject = plateVisualGameObjectList[plateVisualGameObjectList.Count - 1];
        plateVisualGameObjectList.Remove(plateGameObject);
        Destroy(plateGameObject);
    }

    private void PlatesWorkstation_OnPlateSpawned(object sender, System.EventArgs e)
    {
        Transform plateVisualTransform = Instantiate(plateVisualPrefab, workstationTopPoint);

        float plateOffsetY = .1f;
        plateVisualTransform.localPosition = new Vector3(0, plateOffsetY * plateVisualGameObjectList.Count, 0);

        plateVisualGameObjectList.Add(plateVisualTransform.gameObject);
    }
}
