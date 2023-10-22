using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurnaceBurnFlashingBarUI : MonoBehaviour
{
    private const string IS_FLASHING = "IsFlashing";

    [SerializeField] private FurnaceWorkstation furnaceWorkstation;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        furnaceWorkstation.OnProgressChanged += FurnaceWorkstation_OnProgressChanged;

        animator.SetBool(IS_FLASHING, false);
    }

    private void FurnaceWorkstation_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
    {
        float burnShowProgressAmount = .5f;
        bool show = furnaceWorkstation.IsFried() && e.progressNormalized >= burnShowProgressAmount;

        animator.SetBool(IS_FLASHING, show);
    }
}
