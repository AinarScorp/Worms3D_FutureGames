using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace WormsGame.SceneManagement
{ 
    public class SceneHandler : MonoBehaviour
    {
        public void LoadNextScene()
        {
            int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
            StartCoroutine(LoadScene(nextSceneIndex));
        }
        public void LoadFirstScene()
        {
            StartCoroutine(LoadScene(0));

        }
        IEnumerator LoadScene(int sceneIndex)
        {
            yield return null;
            SceneManager.LoadScene(sceneIndex);
        }
        public void QuitGame()
        {
            Application.Quit();
        }
    }
}
