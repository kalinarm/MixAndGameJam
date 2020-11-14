using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MG
{
    public class LevelUI : MonoBehaviour
    {
        LevelData level;
        public Text texLevelTitle;
        public Text texLevelInstruction;
        public Text texLevelIndex;

        public string levelPrefix = "Level";

        void Start()
        {
            refresh();
        }

        [EditorButton]
        public void refresh()
        {
            if (level == null)
            {
                level = GameManager.Instance.level;
            }
            if (texLevelTitle != null) texLevelTitle.text = level.title;
            if (texLevelInstruction != null) texLevelInstruction.text = level.instruction;
            if (texLevelIndex != null) texLevelIndex.text = levelPrefix + " " + level.levelIndex;
        }
    }
}
