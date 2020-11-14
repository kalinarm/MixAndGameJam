using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
namespace MG
{
    [CreateAssetMenu]
    public class AudioTrigger : ScriptableObject
    {
        public enum SOUND_TYPE
        {
            EMIT_3D,
            EMIT_2D
        }

        [Header("General")]
        public AudioClip clip;
        public AudioClip[] clipsRandom;

        public AudioMixerGroup channel;

        [Header("Config")]
        public float volume = 1f;
        public float pitch = 1f;
        public bool useRandomPitch = false;
        public Vector2 randomPitch = new Vector2(0.9f, 1.1f);

        [Header("spatialize")]
        public SOUND_TYPE soundType = SOUND_TYPE.EMIT_3D;
        public float minDistance = 10f;
        public float maxDistance = 100f;

        [Header("VFX")]
        public GameObject vfx;
        public Vector3 vfxPosOffset;

        public AudioClip Clip
        {
            get
            {
                if (clipsRandom.Length > 0)
                {
                    return clipsRandom[Random.Range(0, clipsRandom.Length)];
                }
                return clip;
            }
        }

        public void trigger(float volumeScale = 1f)
        {
            AudioSource.PlayClipAtPoint(Clip, Vector3.zero, volume * volumeScale);
        }

        public void trigger(AudioSource source, float volumeScale = 1f, float pitchScale = 1f)
        {
            configureSource(source, volumeScale, pitchScale);
            source.Play();
        }

        public void trigger(GameObject obj, float volumeScale = 1f, float pitchScale = 1f)
        {
            AudioSource source = obj.AddComponent<AudioSource>();
            float delay = configureSource(source, volumeScale, pitchScale);
            source.Play();
            GlobalAudioManager.Instance.StartCoroutine(routineClearAudioSource(source, delay));
            createVfx(vfx, obj.transform.position);
        }

        void createVfx(GameObject prefab, Vector3 pos)
        {
            if (prefab == null) return;
            GameObject obj = GameObject.Instantiate(prefab);
            obj.transform.position = pos + vfxPosOffset;
            obj.AddComponent<Helpers.Temporary>();
        }

        public float configureSource(AudioSource source, float volumeScale = 1f, float pitchScale = 1f)
        {
            return configureSource(this, source, volumeScale, pitchScale);
        }

        public static float configureSource(AudioTrigger trigger, AudioSource source, float volumeScale = 1f, float pitchScale = 1f)
        {
            source.clip = trigger.Clip;
            source.outputAudioMixerGroup = trigger.channel;
            source.volume = trigger.volume * volumeScale;
            if (trigger.useRandomPitch)
            {
                source.pitch = Random.Range(trigger.randomPitch.x, trigger.randomPitch.y) * trigger.pitch;
            }else
            {
                source.pitch = trigger.pitch * pitchScale;
            }
            

            source.spatialBlend = trigger.soundType == SOUND_TYPE.EMIT_3D ? 1f : 0f;
            source.minDistance = trigger.minDistance;
            source.maxDistance = trigger.maxDistance;
            return source.clip != null ? source.clip.length + 0.5f : 0f;
        }

        #region helper
        public IEnumerator routineClearAudioSource(AudioSource source, float delay)
        {
            yield return new WaitForSeconds(delay);
            GameObject.Destroy(source);
        }
        #endregion
    }
}
