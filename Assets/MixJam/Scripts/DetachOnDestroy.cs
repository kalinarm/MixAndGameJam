using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MG
{
    public class DetachOnDestroy : MonoBehaviour
    {
        public GameObject[] objectsToDetach;

        public void OnDestroy()
        {
            foreach (var item in objectsToDetach)
            {
                item.transform.SetParent(null);
            }
        }
    }
}
