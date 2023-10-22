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
        float burnShowProgressAmount = .5f;
        playWarningSound = furnaceWorkstation.IsFried() && e.progressNormalized >= burnShowProgressAmount;
    }

    private void FurnaceWorkstation_OnStateChanged(object sender, FurnaceWorkstation.OnStateChangedEventArgs e)
    {
        bool playSound = e.state == FurnaceWorkstation.State.Frying || e.state == FurnaceWorkstation.State.Fried;
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
