using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MG
{
    public class Interactor : MonoBehaviour
    {
        public List<Interactable> grabbed = new List<Interactable>();

        [SerializeField] Rigidbody rigid;
        public Rigidbody Rigid { get => rigid; set => rigid = value; }

        public float attachForce = 10f;
        public float attachViscosity = 1f;

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
            foreach (var item in grabbed)
            {
                item.launch(velocity, angularVel);
                item.setGrabbable(true);
            }
            grabbed.Clear();
        }

        public bool hasObjectGrabbed()
        {
            return grabbed.Count > 0;
        }

        public void setWorldPosition(Vector3 pos)
        {
            transform.position = pos;
        }
    }
}
