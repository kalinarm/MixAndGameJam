using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace MG
{
    public class AudioFx : MonoBehaviour
    {
        public AudioTrigger sound;

        public bool emitOnEnable;

        void OnEnable()
        {
            if (emitOnEnable) sound.trigger(gameObject);
        }
    }
}
