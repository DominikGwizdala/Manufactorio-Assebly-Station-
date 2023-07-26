using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCunter : MonoBehaviour
{
    [SerializeField] private KitchenObjectSO kitchenObjectsSO;
    [SerializeField] private Transform counterTopPoint;
    public void Interact()
    {
        Debug.Log("interact");
        Transform kitchenObjectTransform = Instantiate(kitchenObjectsSO.prefab, counterTopPoint);
        kitchenObjectTransform.localPosition = Vector3.zero;
        Debug.Log(kitchenObjectTransform.GetComponent<KitchenObject>().GetKitchenObjectSO().objectName);
    }
}
