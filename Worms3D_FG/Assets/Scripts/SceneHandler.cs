using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace WormsGame.SceneManagement
{ 
    public class SceneHandler : MonoBehaviour
    {
        public void LoadNextScene()
        {
            int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
            SceneManager.LoadScene(nextSceneIndex);
        }
        public void LoadFirstScene()
        {
            SceneManager.LoadScene(0);
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}
