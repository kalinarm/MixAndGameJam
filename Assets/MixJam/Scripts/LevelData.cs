using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MG
{
    [CreateAssetMenu]
    public class LevelData : ScriptableObject
    {
        public string title;
        [Multiline]
        public string instruction;
        public int levelIndex = 0;
    }
}
