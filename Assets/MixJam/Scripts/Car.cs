using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MG
{
    public class Car : Orderable
    {
        public float speed = 1f;
        public float timeAction = 2f;

        public override void onDiceLaunched(DiceInteractable dice, int number, DiceZone zone = null)
        {
            base.onDiceLaunched(dice, number);

            StartCoroutine(routineMove(number));
        }

        IEnumerator routineMove(int number)
        {
            float t = 0f;
            WaitForEndOfFrame wait = new WaitForEndOfFrame();
            while(t<=timeAction * number)
            {
                transform.Translate(Vector3.forward * speed * Time.deltaTime, Space.Self);
                yield return wait;
                t += Time.deltaTime;
            }
        }
    }
}
