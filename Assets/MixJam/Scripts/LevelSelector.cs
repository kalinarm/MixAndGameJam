using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MG
{
    public class LevelSelector : MonoBehaviour
    {
        public List<Button> buttons;

        void OnEnable()
        {
            refresh();
        }

        void refresh()
        {
            var levels = GameManager.Instance.data.levels;
            int currentLevel = GameManager.Instance.getGameIndex();
            
            for (int i = 0; i < buttons.Count; i++)
            {
                if (i >= levels.Count)
                {
                    buttons[i].gameObject.SetActive(false);
                    continue;
                }
                int j = i;
                buttons[i].GetComponentInChildren<Text>().text = levels[i].title;
                buttons[i].interactable = i < currentLevel;
                buttons[i].onClick.RemoveAllListeners();
                buttons[i].onClick.AddListener(()=> onClickLevel(j));
            }
        }

        void onClickLevel(int i)
        {
            Debug.Log("click level " + i);
            GameManager.Instance.loadLevel(i);
        }
        
    }
}
