using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace MG
{
    public class GameUI : MonoBehaviour
    {
        public Button continueGame;
        public Toggle music;

        void OnEnable()
        {
            if (continueGame != null) continueGame.interactable = (GameManager.Instance.canContinueGame());
            if (music != null)
            {
                music.isOn = !MusicBehavior.isMuted();
                music.onValueChanged.AddListener(onClickMute);
            }
        }

        public void restartLevel()
        {
            GameManager.Instance.restartLevel();
        }
        public void nextLevel()
        {
            GameManager.Instance.nextLevel();
        }
        public void clickNewGame()
        {
            GameManager.Instance.onNewGame();
        }
        public void backToMenu()
        {
            GameManager.Instance.backToMenu();
        }
        public void clickContinueGame()
        {
            GameManager.Instance.continueGame();
        }
        public void onClickMute(bool active)
        {
            MusicBehavior.Mute(!active);
        }

    }
}
