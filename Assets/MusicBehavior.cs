using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicBehavior : MonoBehaviour
{
    public static bool musicTriggered;
    AudioSource audioSource;
    public float fadeInTime;

    private void Awake()
    {
        if (!musicTriggered)
        {
            audioSource = GetComponent<AudioSource>();
            DontDestroyOnLoad(gameObject);
            musicTriggered = true;
            VolumeFadingMusic volFadeMusic = GetComponent<VolumeFadingMusic>();
            volFadeMusic.SetInitialVolume(audioSource);
            volFadeMusic.VolumeFade(audioSource, 1, fadeInTime);
            audioSource.Play();
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
