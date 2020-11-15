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

        public Path path;
        public int startingCaseIndex;
        public int currentCaseIndex;
        public int goalCaseIndex;
        bool isBlockedOnPath = false;

        Rigidbody rigid;

        void Start()
        {
            rigid = GetComponent<Rigidbody>();
            currentCaseIndex = startingCaseIndex;
            path = GameObject.FindObjectOfType<Path>();
            if (path != null)
            {
                rigid.constraints = RigidbodyConstraints.FreezeAll;
                StartCoroutine(routinePath());
            }
        }

        public override void onDiceLaunched(DiceInteractable dice, int number, DiceZone zone = null)
        {
            base.onDiceLaunched(dice, number);
            switch (dice.diceType)
            {
                case DiceInteractable.DICE_TYPE.NONE:
                    break;
                case DiceInteractable.DICE_TYPE.MOVE_FORWARD:
                    if (path != null) addToGoalCaseIndex(number);
                    else StartCoroutine(routineMove(number));
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



        public void recoil(Vector3 direction)
        {
            if (path != null)
            {
                isBlockedOnPath = true;
                addToGoalCaseIndex(-1);
            }
            else rigid.AddForce(direction);
        }

        void addToGoalCaseIndex(int i )
        {
            if (isBlockedByObstacle() || isBlockedOnPath) return;
            goalCaseIndex += i;
        }

        IEnumerator routinePath()
        {
            WaitForEndOfFrame wait = new WaitForEndOfFrame();
            yield return StartCoroutine(routineChangeCase(startingCaseIndex));
            while (true)
            {
                yield return wait;
                if (goalCaseIndex != currentCaseIndex)
                {
                    int dif = goalCaseIndex > currentCaseIndex ? 1 : -1;
                    yield return StartCoroutine(routineChangeCase(currentCaseIndex + dif));
                }
            }
        }
        IEnumerator routineChangeCase(int goal)
        {
            Debug.Log("change case for " + goal);
            float t = 0f;
            WaitForEndOfFrame wait = new WaitForEndOfFrame();
            Vector3 dest = path.getCasePos(goal);
            Vector3 pos = path.getCasePos(currentCaseIndex);
            while (t <= timeAction && !isBlockedOnPath && !isBlockedByObstacle())
            {
                transform.position = Vector3.Lerp(pos, dest, t / timeAction);
                yield return wait;
                t += Time.deltaTime;
            }
            if ((dest - transform.position).sqrMagnitude < 0.1f)
            {
                currentCaseIndex = goal;
            }
            
            Vector3 destNext = path.getCasePos(currentCaseIndex + 1);
            if (destNext != dest)
            {
                float angle;
                do
                {
                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(destNext - dest), 3f * turnSpeed * Time.deltaTime);
                    angle = Vector3.Angle(destNext - dest, transform.forward);
                    yield return wait;
                } while (angle > 1f);
            }

            if (isBlockedOnPath || isBlockedByObstacle())
            {
                isBlockedOnPath = false;
                goalCaseIndex = currentCaseIndex;
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

        void setBlockOnPath()
        {
            isBlockedOnPath = true;
        }

        bool isBlockedByObstacle()
        {
            currentTouchObstacle.RemoveAll(x => x == null);
            return currentTouchObstacle.Count > 0;
        }

        List<Obstacle> currentTouchObstacle = new List<Obstacle>();
        void OnCollisionEnter(Collision col)
        {
            Obstacle o = col.collider.GetComponent<Obstacle>();
            if (o != null)
            {
                setBlockOnPath();
                if (!currentTouchObstacle.Contains(o)) currentTouchObstacle.Add(o);
            }
        }
        void OnCollisionExit(Collision col)
        {
            Obstacle o = col.collider.GetComponent<Obstacle>();
            if (o != null)
            {
                setBlockOnPath();
            }
            currentTouchObstacle.RemoveAll(x => x == null || x == o);
        }
    }
}
