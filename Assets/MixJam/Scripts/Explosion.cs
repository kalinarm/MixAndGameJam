﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MG
{
    public class Explosion : MonoBehaviour
    {
        public float range = 1;
        public float duration = 2f;
        public float durationBig = 0.5f;
        public float forceApplied = 10f;

        public float effectiveRange;

        public List<Rigidbody> excludeRigidFromForceApplied = new List<Rigidbody>();

        public AudioTrigger fxStart;

        public void setRange(int number)
        {
            effectiveRange = range * number;
        }

        void Start()
        {
            if (fxStart != null) fxStart.trigger(gameObject);
            StartCoroutine(routine());
        }

        IEnumerator routine()
        {
            WaitForEndOfFrame wait = new WaitForEndOfFrame();
            float tNorm = 0f;
            for (float t = 0; t < duration; t = t + Time.deltaTime)
            {
                tNorm = Mathf.Clamp01(t / duration);
                transform.localScale = Vector3.one * tNorm * effectiveRange;
                yield return wait;
            }
            yield return new WaitForSeconds(durationBig);
            GameObject.Destroy(gameObject);
        }

        void OnTriggerEnter(Collider other)
        {
            Obstacle obstacle = other.GetComponent<Obstacle>();
            if (obstacle != null)
            {
                if (obstacle.isDectructible)
                {
                    obstacle.autodestroy();
                }
            }

            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null && !rb.isKinematic && !excludeRigidFromForceApplied.Contains(rb))
            {
                rb.AddForce(forceApplied * (rb.transform.position - transform.position).normalized, ForceMode.Impulse);
            }
        }
    }
}
