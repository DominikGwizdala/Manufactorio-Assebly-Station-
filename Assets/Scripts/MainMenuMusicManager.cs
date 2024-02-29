using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuMusicManager : MonoBehaviour
{
    private const string PLAYER_PREFS_MUSIC_VOLUME = "MusicVolume";

    public static MainMenuMusicManager Instance { get; private set; }

    private AudioSource audioSource;
    private float volume = .08f;

    private void Awake()
    {
        Instance = this;

        audioSource = GetComponent<AudioSource>();

        volume = PlayerPrefs.GetFloat(PLAYER_PREFS_MUSIC_VOLUME, .08f);
        audioSource.volume = volume;
    }

    public void ChangeVolume()
    {
        volume += .04f;
        if (volume > .4f)
        {
            volume = 0f;
        }
        audioSource.volume = volume;
        PlayerPrefs.SetFloat(PLAYER_PREFS_MUSIC_VOLUME, volume);
        PlayerPrefs.Save();
    }
    public float GetVolume()
    {
        return volume;
    }
}
