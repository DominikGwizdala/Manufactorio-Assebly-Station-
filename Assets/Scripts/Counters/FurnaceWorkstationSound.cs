using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurnaceWorkstationSound : MonoBehaviour
{
    [SerializeField] private FurnaceWorkstation furnaceWorkstation;
    private AudioSource audioSource;
    private float warningSoundTimer;
    private bool playWarningSound;

    private void Awake()
    { 
        audioSource = GetComponent<AudioSource>();
    }
    private void Start()
    {
        furnaceWorkstation.OnStateChanged += FurnaceWorkstation_OnStateChanged;
        furnaceWorkstation.OnProgressChanged += FurnaceWorkstation_OnProgressChanged;
    }

    private void FurnaceWorkstation_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
    {
        float oversmeltingShowProgressAmount = .5f;
        playWarningSound = furnaceWorkstation.IsSmelted() && e.progressNormalized >= oversmeltingShowProgressAmount;
    }

    private void FurnaceWorkstation_OnStateChanged(object sender, FurnaceWorkstation.OnStateChangedEventArgs e)
    {
        bool playSound = e.state == FurnaceWorkstation.State.Smelting || e.state == FurnaceWorkstation.State.Smelted;
        if (playSound)
        {
            audioSource.Play();
        }
        else
        {
            audioSource.Pause();
        }
    }

    private void Update()
    {
        if (playWarningSound)
        {
            warningSoundTimer -= Time.deltaTime;
            if (warningSoundTimer <= 0f)
            {
                float warningSoundTimerMax = .2f;
                warningSoundTimer = warningSoundTimerMax;

                SoundManager.Instance.PlayWarningSound(furnaceWorkstation.transform.position);
            }
        }
    }
}
