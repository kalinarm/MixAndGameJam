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
        public int currentIndex;
        public int goalIndex;
        //public int nextGoalIndex;
        bool isStoppedByProjectile = false;

        Rigidbody rigid;

        List<Obstacle> currentTouchObstacle = new List<Obstacle>();

        public int GoalIndex
        {
            get => goalIndex;
            set
            {
                goalIndex = Mathf.Max(0, value);
                //goalIndex = value;
                //Debug.Log("goalIndex=" + goalIndex + " - " + value);
            }
        }

        public int CurrentIndex
        {
            get => currentIndex;
            set
            {
                currentIndex = Mathf.Max(0, value);
                //currentIndex = value;
                //Debug.Log("currentCaseIndex=" + currentIndex);
            }
        }

        void Start()
        {
            rigid = GetComponent<Rigidbody>();
            CurrentIndex = startingCaseIndex;
            path = GameObject.FindObjectOfType<Path>();
            if (path != null)
            {
                rigid.constraints = RigidbodyConstraints.FreezeAll;
                transform.position = path.getCase(startingCaseIndex).transform.position;
                CurrentIndex = startingCaseIndex;
                GoalIndex = startingCaseIndex;
                //nextGoalIndex = startingCaseIndex;
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
            //Debug.Log("recoil begin goal index " + goalIndex);
            if (path != null)
            {
                GoalIndex = currentIndex - 1;
                isStoppedByProjectile = true;
            }
            else rigid.AddForce(direction);
            //Debug.Log("recoil end goal index " + goalIndex);
        }

        bool isBlocked()
        {
            return (isStoppedByProjectile || isBlockedByObstacle());
        }

        void addToGoalCaseIndex(int i)
        {
            if ((isBlockedByObstacle() || isStoppedByProjectile) && i > 0) return;
            GoalIndex = GoalIndex + i;
        }

        IEnumerator routinePath()
        {
            WaitForEndOfFrame wait = new WaitForEndOfFrame();
            yield return StartCoroutine(routineChangeCase());
            while (true)
            {
                yield return wait;
                if (GoalIndex != CurrentIndex)
                {
                    yield return StartCoroutine(routineChangeCase());
                }
            }
        }
        IEnumerator routineChangeCase()
        {
            while (GoalIndex != currentIndex && !isBlockedByObstacle())
            {
                if (isBlockedByObstacle() && !isStoppedByProjectile)
                {
                    GoalIndex = currentIndex;
                    yield break;
                }
                int nextGoalIndex = CurrentIndex + (GoalIndex > CurrentIndex ? 1 : -1);
                Vector3 dest = path.getCasePos(nextGoalIndex);
                Vector3 pos = path.getCasePos(CurrentIndex);
                yield return StartCoroutine(routineMoveToCase(pos, dest));
                if ((dest - pos).sqrMagnitude > 0.1f)
                {
                    CurrentIndex = nextGoalIndex;
                }else if (!isStoppedByProjectile)
                {
                    GoalIndex = currentIndex;
                }
                yield return StartCoroutine(routineRotateCase(currentIndex + 1));
            }
            if (isBlockedByObstacle())
            {
                GoalIndex = currentIndex;
            }
            if (isStoppedByProjectile)
            {
                isStoppedByProjectile = false;
            }
        }

        IEnumerator routineMoveToCase(Vector3 start, Vector3 dest)
        {
            WaitForEndOfFrame wait = new WaitForEndOfFrame();
            float t = 0f;
            while (t <= timeAction)
            {
                transform.position = Vector3.Lerp(start, dest, t / timeAction);
                yield return wait;
                t += Time.deltaTime;
            }
        }
        IEnumerator routineRotateCase(int nextIndex)
        {
            WaitForEndOfFrame wait = new WaitForEndOfFrame();
            Vector3 destNext = path.getCasePos(nextIndex);
            Vector3 pos = transform.position;
            if (destNext != pos)
            {
                float angle;
                do
                {
                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(destNext - pos), 3f * turnSpeed * Time.deltaTime);
                    angle = Vector3.Angle(destNext - pos, transform.forward);
                    yield return wait;
                } while (angle > 1f);
            }
        }

        IEnumerator routineMove(int number)
        {
            float t = 0f;
            WaitForEndOfFrame wait = new WaitForEndOfFrame();
            float sens = number > 0 ? 1f : -1f;

            while (t <= timeAction * Mathf.Abs(number))
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
            isStoppedByProjectile = true;
        }

        bool isBlockedByObstacle()
        {
            currentTouchObstacle.RemoveAll(x => x == null);
            return currentTouchObstacle.Count > 0;
        }



        void OnCollisionEnter(Collision col)
        {
            Obstacle o = col.collider.GetComponent<Obstacle>();
            if (o != null)
            {
                setBlockOnPath();
                if (!currentTouchObstacle.Contains(o)) currentTouchObstacle.Add(o);
                if (o.caseBack > 0)
                {
                    GoalIndex = currentIndex - o.caseBack;
                    if (o.destroyOnAvatarTouch) o.autodestroy();
                }
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
