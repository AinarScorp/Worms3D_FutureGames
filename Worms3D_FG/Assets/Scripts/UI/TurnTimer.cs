using System;
using System.Collections;
using TMPro;
using UnityEngine;
using WormsGame.Core;

namespace WormsGame.UI
{
    public class TurnTimer : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI _timerDisplay;
        [SerializeField] [Range(1, 99)] float _turnTime;

        Coroutine _tickingTimer;
        TurnHandler _turnHandler;

        void Awake()
        {
            _turnHandler = FindObjectOfType<TurnHandler>();
            _turnHandler.TurnStarted += StartTimer;
            _turnHandler.TurnFinished += StopTimer;

        }

        void OnDestroy()
        {
            _turnHandler.TurnStarted -= StartTimer;
            _turnHandler.TurnFinished -= StopTimer;

        }

        void StopTimer()
        {
            if (_tickingTimer != null)
                StopCoroutine(_tickingTimer);
        }
        void StartTimer()
        {
            StopTimer();

            _tickingTimer = StartCoroutine(Timer());
        }

        IEnumerator Timer()
        {
            float startingTime = _turnTime;
            float remainingTime = startingTime;
            while (remainingTime > 0)
            {
                remainingTime -= Time.deltaTime;
                _timerDisplay.text = string.Format("{0:0}", remainingTime);
                yield return null;
            }

            remainingTime = 0;
            _timerDisplay.text = string.Format("{0:00}", remainingTime);
            _turnHandler.TimeIsUp();
            _tickingTimer = null;
        }
    }
}