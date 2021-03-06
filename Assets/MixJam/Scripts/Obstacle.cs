﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MG
{
    public class Obstacle : MonoBehaviour
    {
        public AudioTrigger fxOnDestroy;
        public bool isDectructible = true;
        public int caseBack = 0;
        public bool destroyOnAvatarTouch = false;

        public void autodestroy()
        {
            if (fxOnDestroy != null) fxOnDestroy.trigger(gameObject);
            GameObject.Destroy(gameObject);
        }
    }
}
