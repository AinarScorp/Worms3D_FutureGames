using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WormsGame.UI;

namespace WormsGame.MainMenu
{
    public class MainMenuDecor : MonoBehaviour
    {
        TransitionController _transitionController;
        [SerializeField] Animator _boysAnimator;
        [SerializeField] string _boysUpTriggerString, _boysDownTriggerString;

        void Awake()
        {
            _transitionController = FindObjectOfType<TransitionController>();
            _transitionController.SceneHasOpened += TriggerBoysGoUp;
        }

        void OnDestroy()
        {
            _transitionController.SceneHasOpened -= TriggerBoysGoUp;
        }

        void TriggerBoysGoUp()
        {
            _boysAnimator.SetTrigger(_boysUpTriggerString);
        }
        void TriggerBoysGoDown()
        {
            _boysAnimator.SetTrigger(_boysDownTriggerString);
        }
        
    }
    
}
