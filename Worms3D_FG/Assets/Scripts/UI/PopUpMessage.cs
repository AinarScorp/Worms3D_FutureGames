using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace WormsGame.UI
{
    
    public class PopUpMessage : MonoBehaviour
    {
        public static PopUpMessage Instance { get; private set; }
        [SerializeField] Image _maskImage;
        [SerializeField] float _fillUpSpeed;
        [SerializeField] TextMeshProUGUI _messageDisplay;

        Coroutine _displayingMessage;
        string _message = "___";
        void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this.gameObject);
            }
            Instance = this; 
        }

        void Start()
        {
            SwitchMessageDisplay(false);
        }
        

        public void DisplayPopUpMessage(string message, float turnOffTimer)
        {
            if (_message == message) return;
            if (_displayingMessage !=null) StopCoroutine(_displayingMessage);
            _message = message;
            _messageDisplay.text = message;
            _displayingMessage = StartCoroutine(StartDisplayingMessage(turnOffTimer));
        }

        IEnumerator StartDisplayingMessage(float turnOffTimer)
        {
            SwitchMessageDisplay(true);
            
            //FillUpText();

            float percent = 0;
            while (percent < 1)
            {
                percent += Time.deltaTime * _fillUpSpeed;
                _maskImage.fillAmount = Mathf.Lerp(0, 1, percent);
                yield return null;
            }
            _maskImage.fillAmount = 1;
            yield return new WaitForSeconds(turnOffTimer);
            while (percent > 0)
            {
                percent -= Time.deltaTime * _fillUpSpeed;
                _maskImage.fillAmount = Mathf.Lerp(0, 1, percent);
                yield return null;
            }
            SwitchMessageDisplay(false);
            _displayingMessage = null;
            _message = "___";

        }

        void FillUpText()
        {
            if (_maskImage == null) return;

            float percent = 0;
            while (percent < 1)
            {
                percent += Time.deltaTime * _fillUpSpeed;
                _maskImage.fillAmount = Mathf.Lerp(0, 1, percent);
                
            }
            _maskImage.fillAmount = 1;

        }

        void SwitchMessageDisplay(bool turnOn)
        {
            if (_messageDisplay ==null) return;
            _messageDisplay.gameObject.SetActive(turnOn);

        }
        
    }
}
