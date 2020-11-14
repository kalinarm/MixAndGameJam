using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MG
{
    [CreateAssetMenu]
    public class GameManagerData : ScriptableObject
    {
        public List<LevelData> levels = new List<LevelData>();
        public Explosion diceExplosion;
        public Shield diceShield;

        public GameObject fxDicePickNumber;

        public AudioTrigger audioWin;
        public AudioTrigger audioLoose;
    }
}
