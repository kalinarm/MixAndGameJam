using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MG
{
    public class GlobalAudioManager : MonoBehaviour
    {
        #region singleton
        static GlobalAudioManager instance;
        public static GlobalAudioManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = GameObject.FindObjectOfType<GlobalAudioManager>();
                    if (instance == null)
                    {
                        instance = new GameObject("AudioManager").AddComponent<GlobalAudioManager>();
                    }
                }
                return instance;
            }
        }
        #endregion

        public void triggerEffect(AudioTrigger fx)
        {

        }

        public IEnumerator routineClearAudioSource(AudioSource source, float delay)
        {
            yield return new WaitForSeconds(delay);
            GameObject.Destroy(source);
        }
    }
}
