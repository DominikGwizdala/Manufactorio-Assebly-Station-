using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private const string PLAYER_PREFS_SOUND_EFFECT_VOLUME = "SoundEffectsVolume";

    public static SoundManager Instance{ get; private set; }
    [SerializeField] private AudioClipRefsSO audioClipRefsSO;
    private float volume = .1f;
    private void Awake()
    {
        Instance = this;

        volume = PlayerPrefs.GetFloat(PLAYER_PREFS_SOUND_EFFECT_VOLUME, 1f);
    }
    private void Start()
    {
        DeliveryManager.Instance.OnRecipeSuccess += DeliveryManager_OnRecipeSuccess;
        DeliveryManager.Instance.OnRecipeFailed += DeliveryManager_OnRecipeFailed;
        LogCutterWorkstation.OnAnyCut += LogCutterWorkstation_OnAnyCut;
        AnvilWorkstation.OnAnyForge += AnvilWorkstation_OnAnyForge;
        Player.Instance.OnPickedSomething += Player_OnPickedSomething;
        BaseWorkstation.OnAnyObjectPlacedHere += BaseWorkstation_OnAnyObjectPlacedHere;
        TrashWorkstation.OnAnyObjectTrashed += TrashWorkstation_OnAnyObjectTrashed;         

    }

    private void TrashWorkstation_OnAnyObjectTrashed(object sender, System.EventArgs e)
    {
        TrashWorkstation trashWorkstation = sender as TrashWorkstation;
        PlaySound(audioClipRefsSO.trash, trashWorkstation.transform.position);

    }
    private void BaseWorkstation_OnAnyObjectPlacedHere(object sender, System.EventArgs e)
    {
        BaseWorkstation baseWorkstation = sender as BaseWorkstation;
        PlaySound(audioClipRefsSO.objectDrop, baseWorkstation.transform.position);
    
    }

    private void Player_OnPickedSomething(object sender, System.EventArgs e)
    {
        PlaySound(audioClipRefsSO.objectPickup,Player.Instance.transform.position);
    }

    private void LogCutterWorkstation_OnAnyCut(object sender, System.EventArgs e)
    {
        LogCutterWorkstation logCutterWorkstation = sender as LogCutterWorkstation;
        PlaySound(audioClipRefsSO.chop, logCutterWorkstation.transform.position);
    }

    private void AnvilWorkstation_OnAnyForge(object sender, System.EventArgs e)
    {
        AnvilWorkstation anvilWorkstation = sender as AnvilWorkstation;
        PlaySound(audioClipRefsSO.chop, anvilWorkstation.transform.position);
    }

    private void DeliveryManager_OnRecipeFailed(object sender, System.EventArgs e)
    {
        DeliveryWorkstation deliveryWorkstation = DeliveryWorkstation.Instance;
        PlaySound(audioClipRefsSO.deliveryFail, deliveryWorkstation.transform.position);
    }   

    private void DeliveryManager_OnRecipeSuccess(object sender, System.EventArgs e)
    {
        DeliveryWorkstation deliveryWorkstation = DeliveryWorkstation.Instance;
        PlaySound(audioClipRefsSO.deliverySuccess, deliveryWorkstation.transform.position);
    }
    private void PlaySound(AudioClip[] audioClipArray,Vector3 position,float volume = 1f )
    {
        PlaySound(audioClipArray[Random.Range(0,audioClipArray.Length)],position,volume);
        
    }
    private void PlaySound(AudioClip audioClip, Vector3 position, float volumeMultiplier = 1f)
    {
        AudioSource.PlayClipAtPoint(audioClip, position, volumeMultiplier * volume);
    }
    public void PlayFootstepsSound(Vector3 position,float volume)
    {
        PlaySound(audioClipRefsSO.footstep, position, volume);
    }
    public void PlayCountdownSound()
    {
        PlaySound(audioClipRefsSO.warning, Vector3.zero);
    }
    public void PlayWarningSound(Vector3 position)
    {
        PlaySound(audioClipRefsSO.warning, position);
    }
    public void ChangeVolume() {
        volume += .1f;
        if (volume > 1) {
            volume = 0f;
        }

        PlayerPrefs.SetFloat(PLAYER_PREFS_SOUND_EFFECT_VOLUME, volume);
        PlayerPrefs.Save();
    }
    public float GetVolume()
    {
        return volume;
    }
}
