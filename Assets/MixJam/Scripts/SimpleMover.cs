using UnityEngine;
using System.Collections;

namespace Helpers
{
    public class SimpleMover : MonoBehaviour
    {
        public float speedFactor = 1f;
        public bool useWorld = false;
        public Vector3 translate;
        public Vector3 rotate;
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            float dt = Time.deltaTime;

            transform.Translate(dt * translate * speedFactor, useWorld ? Space.World : Space.Self);
            transform.Rotate(dt * rotate * speedFactor, useWorld ? Space.World : Space.Self);

        }

        public void setSpeedFactor(float val)
        {
            speedFactor = val;
        }
    }
}

