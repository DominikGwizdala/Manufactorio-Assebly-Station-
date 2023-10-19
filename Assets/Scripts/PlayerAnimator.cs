using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private const string IS_RUNNING = "IsRunning";
    private const string IS_HOLDING = "IsHolding";
    //private const string IS_WALKING = "IsWalking";
    private Animator animator;
    [SerializeField] Player player;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        
    }
    private void Update()
    {
        //animator.SetBool(IS_WALKING, player.IsWalking());
        animator.SetBool(IS_RUNNING, player.IsRunning());
        animator.SetBool(IS_HOLDING, player.IsHolding());
    }
}
