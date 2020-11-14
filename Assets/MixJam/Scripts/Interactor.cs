using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace MG
{
    public class Interactor : MonoBehaviour
    {
        public List<Interactable> grabbed = new List<Interactable>();
        public List<Interactable> lastLaunched = new List<Interactable>();

        [SerializeField] Rigidbody rigid;
        public Rigidbody Rigid { get => rigid; set => rigid = value; }

        public float attachForce = 10f;
        public float attachViscosity = 1f;

        public float timeLaunch = 0f;

        void Start()
        {
            rigid = GetComponent<Rigidbody>();
        }
        void Update()
        {

        }

        public void addGrabbedObject(Interactable obj)
        {
            obj.grab();
            grabbed.Add(obj);
            obj.attach(this);
        }

        public void launchGrabbedObject(Vector3 velocity, float angularVel = 0f)
        {
            lastLaunched.Clear();
            foreach (var item in grabbed)
            {
                item.launch(velocity, angularVel);
                item.setGrabbable(true);
                lastLaunched.Add(item);
            }
            grabbed.Clear();

            StopCoroutine("routineLaunch");
            StartCoroutine(routineLaunch());
        }

        public void influenceLastLaunchedObject(Vector3 velocity, float timeInfluence)
        {
            foreach (var item in lastLaunched)
            {
                item.influenceVelocity(velocity, timeLaunch, timeInfluence);
            }
        }

        public bool hasObjectGrabbed()
        {
            return grabbed.Count > 0;
        }

        public void setWorldPosition(Vector3 pos)
        {
            transform.position = pos;
        }

        IEnumerator routineLaunch()
        {
            
            WaitForEndOfFrame wait = new WaitForEndOfFrame();
            List<Interactable> toRemove = new List<Interactable>();
            timeLaunch = 0f;
            while (lastLaunched.Count > 0)
            {
                foreach (var item in lastLaunched)
                {
                    if (!item.isInLaunch) toRemove.Add(item);
                }
                lastLaunched.RemoveAll(x => toRemove.Contains(x));
                toRemove.Clear();
                yield return wait;
                timeLaunch += Time.deltaTime;
            }
        }
    }
}
