using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MG
{
    public class GoalDestination : MonoBehaviour
    {
        public AudioTrigger onWin;
        
        public void triggerFx()
        {
            if (onWin != null) onWin.trigger(gameObject);
        }
    }
}
