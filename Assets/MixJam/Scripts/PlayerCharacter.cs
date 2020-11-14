using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MG
{
    public class PlayerCharacter : MonoBehaviour
    {

        void OnTriggerEnter(Collider other)
        {
            GoalDestination goal = other.GetComponent<GoalDestination>();
            if (goal != null)
            {
                GameManager.Events.Trigger(new Evt.PlayerAtGoal(this, goal));
            }
        }
    }
}
