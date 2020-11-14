using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MG
{
    public class Ennemy : MonoBehaviour
    {
        public float speed = 1f;
        public float turnSpeed = 1f;

        Transform target;

        void Start()
        {
            target = GameManager.Instance.getPlayerAvatar().transform;
        }

        void Update()
        {
            if (target == null) return;


        }
    }
}
