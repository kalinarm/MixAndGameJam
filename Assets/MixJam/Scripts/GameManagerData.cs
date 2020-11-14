using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MG
{
    [CreateAssetMenu]
    public class GameManagerData : ScriptableObject
    {
        public Explosion diceExplosion;

        public GameObject fxDicePickNumber;

        public AudioTrigger audioWin;
        public AudioTrigger audioLoose;
    }
}
