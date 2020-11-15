using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MG
{
    public class SoundTester : MonoBehaviour
    {
        public AudioTrigger audioTrigger;

        public void PlayAudioTrigger()
        {
            AudioSource audioSource = GetComponent<AudioSource>();
            if (audioSource != null)
                DestroyImmediate(audioSource);
            Debug.Log("play sound");
            audioTrigger.trigger(gameObject);
        }



        public float target;
        public float fTime;
        public void FadeVolumeTest()
        {
            GetComponent<VolumeFading>().VolumeFade(audioTrigger, GetComponent<AudioSource>(), target, fTime);
        }

    }
}
