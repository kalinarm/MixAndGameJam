using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace MG
{
    public class Path : MonoBehaviour
    {
        public List<PathCase> cases = new List<PathCase>();
        void Start()
        {
            getChildrens();
        }
        [EditorButton]
        void getChildrens()
        {
            cases = gameObject.GetComponentsInChildren<PathCase>().ToList();
        }
        public PathCase getCase(int i)
        {
            int j = Mathf.Clamp(i, 0, cases.Count - 1);
            return cases[j];
        }
        public Vector3 getCasePos(int i)
        {
            return getCase(i).transform.position;
        }
    }
}
