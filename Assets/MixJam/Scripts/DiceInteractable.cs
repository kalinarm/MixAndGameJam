using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MG
{
    public class DiceInteractable : Interactable
    {
        public enum DICE_TYPE
        {
            NONE,
            MOVE_FORWARD,
            TURN_LEFT,
            TURN_RIGHT,
            BRAKE,
            JUMP,
            SHOOT,
            BOOM
        }
        [Header("Dice")]
        public string id;
        public DICE_TYPE diceType = DICE_TYPE.NONE;
        public Color color = Color.white;
        public int number = 0;

        [SerializeField] List<DiceZone> currentZones = new List<DiceZone>();
        bool isInLaunch = false;

        public override void Start()
        {
            base.Start();
        }

        public override void OnLaunched()
        {
            stopLaunch();
            StartCoroutine(getNumberRoutine());
        }

        public override void attach(Interactor interactor)
        {
            stopLaunch();
            base.attach(interactor);
        }

        DiceZone getZone()
        {
            if (currentZones.Count == 0) return null;
            return currentZones[currentZones.Count - 1];
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

        void stopLaunch()
        {
            StopAllCoroutines();
            isInLaunch = false;
        }

        IEnumerator getNumberRoutine()
        {
            isInLaunch = true;
            WaitForEndOfFrame wait = new WaitForEndOfFrame();
            float angularVel = 1f;
            do
            {
                yield return wait;
                angularVel = rigid.angularVelocity.magnitude;
            } while (angularVel > 0.01f);
            number = pickNumber();
            Debug.Log("dice stopped : picking a number : " + number);
            GameManager.Events.Trigger(new Evt.DicePickNumber(this, number, getZone()));
            audioGrounded.trigger(gameObject);
        }

        #region collision
        void OnCollisionEnter(Collision col)
        {
            float volumeScale = Mathf.InverseLerp(0f, 25f, col.relativeVelocity.magnitude);
            DiceInteractable dice = col.gameObject.GetComponent<DiceInteractable>();
            if (dice != null)
            {
                audioCollideSelf.trigger(gameObject, volumeScale);
                return;
            }

            Obstacle obstacle = col.gameObject.GetComponent<Obstacle>();
            if (obstacle != null)
            {
                
                return;
            }

            audioCollideWall.trigger(gameObject, volumeScale);
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
            audioZoneEnter.trigger(gameObject);
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
            audioZoneExit.trigger(gameObject);
        }
        #endregion
    }
}