using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeFadingMusic : MonoBehaviour
{
    public AnimationCurve fadeCurve;

    float volOriginal;
    float vol;

    IEnumerator volFade;
    float volFadeStart;
    float volFadePath;
    float timeFadeDuration;
    float timeFadeStart;
    float timeProgress;

    public void SetInitialVolume(AudioSource audioSource)
    {
        volOriginal = audioSource.volume;
    }

    public void VolumeFade(AudioSource audioSource, float targetVol, float fadeTime) // configures and triggers the fade procedure
    {
        volFadeStart = vol;
        volFadePath = targetVol * volOriginal - volFadeStart; // targetVol=1, the target is not 1, but is equal to volumeStart

        timeFadeDuration = fadeTime;
        timeFadeStart = Time.time;

        if (volFade != null)
            StopCoroutine(volFade);

        volFade = VolFade(audioSource);
        StartCoroutine(volFade);
    }
    IEnumerator VolFade(AudioSource audioSource) // runs every frame, until fade procedure is done
    {
        bool stopFading = false;
        while (!stopFading)
        {
            timeProgress = (Time.time - timeFadeStart) / (timeFadeDuration);
            if (timeProgress > 1)
            {
                timeProgress = 1;
                stopFading = true;
            }
            if (timeProgress < 0)
            {
                timeProgress = 0;
                stopFading = true;
            }
            vol = volFadeStart + fadeCurve.Evaluate(timeProgress) * volFadePath;

            audioSource.volume = vol;

            yield return null;
        }
    }
}