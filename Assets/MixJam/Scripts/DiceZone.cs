using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MG
{
    public class DiceZone : MonoBehaviour
    {
        public enum ZONE_EFFECT
        {
            NOTHING,
            DICE_PLUS,
            DICE_MINUS,
            DICE_MULTIPLICATOR
        }

        [System.Serializable]
        public class DiceZoneParam
        {
            public ZONE_EFFECT effect = ZONE_EFFECT.NOTHING;
            public int effectParameter = 0;
        }

        public DiceZoneParam config = new DiceZoneParam();
    }
}
