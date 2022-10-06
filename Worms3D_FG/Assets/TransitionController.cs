using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WormsGame.UI
{
    
    public class TransitionController : MonoBehaviour
    {
        [SerializeField] Image _image;
        [SerializeField] string _sceneOpenerTriggerString, _sceneCloserTriggerString;
        public event Action SceneHasOpened;
        public event Action SceneHasClosed;

        Animator _animator;

        void Awake()
        {
            _animator = GetComponent<Animator>();
            _image.raycastTarget = true;

        }
        
        public void TriggerSceneOpening()
        {
            _image.gameObject.SetActive(true);

            _animator.SetTrigger(_sceneOpenerTriggerString);
        }
        public void TriggerSceneCloser()
        {
            _image.gameObject.SetActive(true);

            _animator.SetTrigger(_sceneCloserTriggerString);
        }
        void SceneOpened()
        {
            _image.raycastTarget = false;
            _image.gameObject.SetActive(false);

            SceneHasOpened?.Invoke();
        }
        void SceneClosed()
        {
            _image.raycastTarget = true;

            SceneHasClosed?.Invoke();
        }
    }
}
