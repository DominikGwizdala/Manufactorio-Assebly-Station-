using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerWorkstationVisual : MonoBehaviour
{
    private const string OPEN_CLOSE = "OpenClose";

    [SerializeField] private ContainerWorkstation containerWorkstation;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        containerWorkstation.OnPlayerGrabbedObject += ContainerWorkstation_OnPlayerGrabbedObject;
    }
    private void ContainerWorkstation_OnPlayerGrabbedObject(object sender, System.EventArgs e)
    {
        animator.SetTrigger(OPEN_CLOSE);
    }
}
