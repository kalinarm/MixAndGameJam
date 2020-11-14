using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MG
{
    public class Avatar : Orderable
    {
        public float speed = 1f;
        public float turnSpeed = 1f;
        public float timeAction = 2f;
        public float diceFactor = 1f;

        Rigidbody rigid;

        void Start()
        {
            rigid = GetComponent<Rigidbody>();
        }

        public override void onDiceLaunched(DiceInteractable dice, int number, DiceZone zone = null)
        {
            base.onDiceLaunched(dice, number);
            switch (dice.diceType)
            {
                case DiceInteractable.DICE_TYPE.NONE:
                    break;
                case DiceInteractable.DICE_TYPE.MOVE_FORWARD:
                    StartCoroutine(routineMove(number));
                    break;
                case DiceInteractable.DICE_TYPE.TURN_LEFT:
                    StartCoroutine(routineTurn(number, -1));
                    break;
                case DiceInteractable.DICE_TYPE.TURN_RIGHT:
                    StartCoroutine(routineTurn(number, 1));
                    break;
                case DiceInteractable.DICE_TYPE.BRAKE:
                    break;
                case DiceInteractable.DICE_TYPE.JUMP:
                    break;
                case DiceInteractable.DICE_TYPE.SHOOT:
                    break;
                default:
                    break;
            }

        }

        IEnumerator routineMove(int number)
        {
            float t = 0f;
            WaitForEndOfFrame wait = new WaitForEndOfFrame();
            float sens = number > 0 ? 1f : -1f;
            while(t<=timeAction * Mathf.Abs(number))
            {
                //transform.Translate(Vector3.forward * speed * Time.deltaTime * sens, Space.Self);
                rigid.velocity = transform.forward * speed * sens * diceFactor;
                yield return wait;
                t += Time.deltaTime;
            }
        }
        IEnumerator routineTurn(int number, int left)
        {
            float t = 0f;
            WaitForEndOfFrame wait = new WaitForEndOfFrame();
            while (t <= timeAction * number)
            {
                //transform.Rotate(Vector3.up * turnSpeed * left * Time.deltaTime, Space.Self);
                rigid.angularVelocity = Vector3.up * turnSpeed * left * diceFactor;
                yield return wait;
                t += Time.deltaTime;
            }
        }
    }
}
