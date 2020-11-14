using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MG
{
    public class StartZone : MonoBehaviour
    {
        public DiceInteractable dice;
        public Colorizer colorFull = new Colorizer();
        public float alpha = 0.4f;
        public Colorizer colorWithAlpha = new Colorizer();
        public Text text;


        void Start()
        {
            configure();
        }

        [EditorButton]
        void configure()
        {
            if (dice == null) return;
            if (text != null) text.text = dice.id;
            Color c = dice.color;
            colorFull.setColor(c);
            c.a = alpha;
            colorWithAlpha.setColor(c);
        }
    }
}
