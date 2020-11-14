using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MG
{
    public class GameUI : MonoBehaviour
    {
        public void restartLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        public void nextLevel()
        {
            int index = SceneManager.GetActiveScene().buildIndex + 1;
            if (index > SceneManager.sceneCountInBuildSettings) return;
            SceneManager.LoadScene(index);
        }
        public void backToMenu()
        {
            SceneManager.LoadScene(0);
        }
    }
}
