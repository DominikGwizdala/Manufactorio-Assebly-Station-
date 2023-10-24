using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurnaceOversmeltingFlashingBarUI : MonoBehaviour
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
        float oversmeltingShowProgressAmount = .5f;
        bool show = furnaceWorkstation.IsSmelted() && e.progressNormalized >= oversmeltingShowProgressAmount;

        animator.SetBool(IS_FLASHING, show);
    }
}
