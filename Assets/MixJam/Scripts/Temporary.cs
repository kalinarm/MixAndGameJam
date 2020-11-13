﻿using UnityEngine;
using System.Collections;

namespace Helpers
{
    public class Temporary : MonoBehaviour
    {
        public float duration = 10f;
        // Use this for initialization
        void Start()
        {
            Invoke("autodestroy", duration);
        }

        // Update is called once per frame
        void autodestroy()
        {
            GameObject.Destroy(this.gameObject);
        }
    }
}

