using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackageWorkstationVisual : MonoBehaviour
{
    [SerializeField] private PackageWorkstation packageWorkstation;
    [SerializeField] private Transform workstationTopPoint;
    [SerializeField] private Transform packageVisualPrefab;

    private List<GameObject> packageVisualGameObjectList;

    private void Awake()
    {
        packageVisualGameObjectList = new List<GameObject>();
    }

    private void Start()
    {
        packageWorkstation.OnPackageSpawned += PackageWorkstation_OnPackageSpawned;
        packageWorkstation.OnPackageRemoved += PackageWorkstation_OnPackageRemoved;
    }

    private void PackageWorkstation_OnPackageRemoved(object sender, System.EventArgs e)
    {
        GameObject packageGameObject = packageVisualGameObjectList[packageVisualGameObjectList.Count - 1];
        packageVisualGameObjectList.Remove(packageGameObject);
        Destroy(packageGameObject);
    }

    private void PackageWorkstation_OnPackageSpawned(object sender, System.EventArgs e)
    {
        Transform packageVisualTransform = Instantiate(packageVisualPrefab, workstationTopPoint);

        float packageOffsetY = .1f;
        packageVisualTransform.localPosition = new Vector3(0, packageOffsetY * packageVisualGameObjectList.Count, 0);

        packageVisualGameObjectList.Add(packageVisualTransform.gameObject);
    }
}
