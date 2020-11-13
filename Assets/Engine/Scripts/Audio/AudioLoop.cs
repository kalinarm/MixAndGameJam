using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
namespace MG
{
   /* [CreateAssetMenu]
    public class AudioLoop : ScriptableObject
    {
        [Header("General")]
        public AudioClip clip;
        public AudioMixer channel;
        [Header("Config")]
        public float volume = 1f;
        public float pitch = 1f;
    }*/

    public class AudioLoopFx : MonoBehaviour
    {
        public AudioSource source;
        public AudioTrigger fx;

        void OnEnable()
        {
            if (source == null)
            {
                source = gameObject.AddComponent<AudioSource>();
            }
            AudioTrigger.configureSource(fx, source);
            source.loop = true;
            source.Play();
        }
    }
}