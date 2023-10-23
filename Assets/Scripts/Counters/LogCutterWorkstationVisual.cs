using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogCutterWorkstationVisual : MonoBehaviour
{
    private const string CUT = "Cut";

    [SerializeField] private LogCutterWorkstation logCutterWorkstation;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        logCutterWorkstation.OnCut += logCutterWorkstation_OnCut;
    }

    private void logCutterWorkstation_OnCut(object sender, System.EventArgs e)
    {
        animator.SetTrigger(CUT);
    }
}
