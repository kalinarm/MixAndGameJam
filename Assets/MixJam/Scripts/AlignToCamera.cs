using UnityEngine;
using System.Collections;

namespace Helpers
{
    public class AlignToCamera : MonoBehaviour
    {
        public bool horizontal = false;
        public bool alignAtUpdate = true;

        // Use this for initialization
        void Awake()
        {
            apply();
        }

        // Update is called once per frame
        void Update()
        {
            if (alignAtUpdate)
                apply();
        }

        void apply()
        {
            if (horizontal)
            {
                //Vector3 p = Camera.main.transform.position - transform.position;
                //transform.LookAt (transform.position - p, transform.up);
                transform.LookAt(Camera.main.transform.position, Vector3.up);
            }
            else
            {
                transform.LookAt(Camera.main.transform, transform.up);
            }
        }
    }
}


