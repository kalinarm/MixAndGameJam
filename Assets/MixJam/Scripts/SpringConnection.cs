using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MG
{
    public class SpringConnection : MonoBehaviour
    {
        public Rigidbody rigid;
        public Rigidbody connectedBody;
        public float force = 1f;
        public float damping = 0.1f;
        public float minDistance = 0.1f;
        void Start()
        {
            rigid = GetComponent<Rigidbody>();
        }
        void Update()
        {
            Vector3 dir = connectedBody.transform.position - rigid.transform.position;
            float dist = dir.magnitude;

            if (dist <= minDistance) return;

            Vector3 f = dir * force * Time.deltaTime 
                + (connectedBody.velocity - rigid.velocity) * damping * Time.deltaTime * Time.deltaTime;
            rigid.AddForce(f);

        }
    }
}
