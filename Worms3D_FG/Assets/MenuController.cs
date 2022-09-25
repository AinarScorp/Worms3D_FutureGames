using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MenuController : MonoBehaviour
{
    [SerializeField] SceneHandler _sceneHandler;

    public void StartTheGame()
    {
        _sceneHandler.LoadNextScene();
    }
    public void SwitchSections(SwitchSectionBtn switchBtn)
    {
        switchBtn.SwitchSections();
    }
}
