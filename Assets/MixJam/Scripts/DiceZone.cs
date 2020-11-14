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

        public int getCorrectNumber(int input)
        {
            int output = input;
            switch (config.effect)
            {
                case ZONE_EFFECT.DICE_PLUS:
                    output = input + config.effectParameter;
                    break;
                case ZONE_EFFECT.DICE_MINUS:
                    output = input - config.effectParameter;
                    break;
                case ZONE_EFFECT.DICE_MULTIPLICATOR:
                    output = input * config.effectParameter;
                    break;
                default:
                    break;
            }
            return output;
        }

        public string getString()
        {
            string s = "";
            switch (config.effect)
            {
                case ZONE_EFFECT.DICE_PLUS:
                    s = " + ";
                    break;
                case ZONE_EFFECT.DICE_MINUS:
                    s = " - ";
                    break;
                case ZONE_EFFECT.DICE_MULTIPLICATOR:
                    s = " x ";
                    break;
                default:
                    break;
            }
            return s + config.effectParameter.ToString();
        }
    }
}
