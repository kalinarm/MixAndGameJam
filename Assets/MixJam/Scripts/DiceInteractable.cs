using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MG
{
    public class DiceInteractable : Interactable
    {
        public string id;
        public Color color = Color.white;
        public int number = 0;

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
                return 3;
            if (Vector3.Dot(-transform.up, Vector3.up) > 0.6f)
                return 4;
            if (Vector3.Dot(transform.right, Vector3.up) > 0.6f)
                return 6;
            if (Vector3.Dot(-transform.right, Vector3.up) > 0.6f)
                return 1;
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
    }
}