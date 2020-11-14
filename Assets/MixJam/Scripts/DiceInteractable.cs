using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MG
{
    public class DiceInteractable : Interactable
    {
        [Header("Dice")]
        public string id;
        public Color color = Color.white;
        public int number = 0;

        [SerializeField] List<DiceZone> currentZones = new List<DiceZone>();

        public override void Start()
        {
            base.Start();
        }

        public override void OnLaunched()
        {
            StopAllCoroutines();
            StartCoroutine(getNumberRoutine());
        }

        int pickNumber()
        {
            if (Vector3.Dot(transform.forward, Vector3.up) > 0.6f)
                return 5;
            if (Vector3.Dot(-transform.forward, Vector3.up) > 0.6f)
                return 2;
            if (Vector3.Dot(transform.up, Vector3.up) > 0.6f)
                return 6;
            if (Vector3.Dot(-transform.up, Vector3.up) > 0.6f)
                return 1;
            if (Vector3.Dot(transform.right, Vector3.up) > 0.6f)
                return 4;
            if (Vector3.Dot(-transform.right, Vector3.up) > 0.6f)
                return 3;
            return 0;
        }

        IEnumerator getNumberRoutine()
        {
            WaitForEndOfFrame wait = new WaitForEndOfFrame();
            float angularVel = 1f;
            do
            {
                yield return wait;
                angularVel = rigid.angularVelocity.magnitude;
            } while (angularVel > 0.01f);
            number = pickNumber();
            Debug.Log("dice stopped : picking a number : " + number);
            GameManager.Events.Trigger(new Evt.DicePickNumber(this, number));
        }

        void OnTriggerEnter(Collider other)
        {
            DiceZone zone = other.GetComponent<DiceZone>();
            if (zone != null)
            {
                if (!currentZones.Contains(zone))
                {
                    currentZones.Add(zone);
                }
            }
        }

        void OnTriggerExit(Collider other)
        {
            DiceZone zone = other.GetComponent<DiceZone>();
            if (zone != null)
            {
                if (currentZones.Contains(zone))
                {
                    currentZones.Remove(zone);
                }
            }
        }
    }
}