using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MG
{
    public class ProjectileLauncher : MonoBehaviour
    {
        public GameObject prefab;
        public Transform shootPos;
        public float shootPeriod = 5f;

        void Start()
        {
            InvokeRepeating("shoot", shootPeriod, shootPeriod);
        }

        void shoot()
        {
            GameObject obj = GameObject.Instantiate(prefab);
            obj.transform.position = shootPos.position;
            obj.transform.rotation = shootPos.rotation;
        }
    }
}
