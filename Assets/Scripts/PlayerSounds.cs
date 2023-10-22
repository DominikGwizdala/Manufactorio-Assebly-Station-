using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    private Player player;
    private float footstepsTimer;
    private float footstepsTimerMax = .1f;

    private void Awake()
    {
       player = GetComponent<Player>(); 
    }

    private void Update()
    {
        footstepsTimer -= Time.deltaTime;
        if (footstepsTimer < 0f) { 
            footstepsTimer = footstepsTimerMax;
            if (player.IsRunning())
            {
                float volume = 1f;
                SoundManager.Instance.PlayFootstepsSound(player.transform.position, volume);
            }
        }
    }
}
