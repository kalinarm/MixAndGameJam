using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PermanentMusic : MonoBehaviour
{
    public static bool musicTriggered;

    private void Awake()
    {
        if (!musicTriggered)
        {
            DontDestroyOnLoad(gameObject);
            musicTriggered = true;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
