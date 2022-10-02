using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WormsGame.Movement
{
    public class ImpactKnockback : MonoBehaviour
    {
        [SerializeField] bool _update = false;
        [SerializeField] Vector3 _direction;
        [SerializeField] float _force;
        Vector3 _impact = Vector3.zero;
        CharacterController _characterController;
        [SerializeField] float _impactReductionSpeed = 2.0f;
        Coroutine _impactReduction;

        public Vector3 Impact => _impact;

        void Update()
        {
            if (!_update) return;
            _update = false;
            AddImpact(_direction.normalized, _force);
        }

        public void AddImpact(Vector3 direction,float force)
        {
            direction.Normalize();
            _impact += direction * force;
            if (_impactReduction !=null) StopCoroutine(_impactReduction);
            
            _impactReduction = StartCoroutine(ReduceImpactVector());
        }

        IEnumerator ReduceImpactVector()
        {
            Vector3 startingImpact = _impact;
            float percent = 0;
            while (percent <1)
            {
                percent += Time.deltaTime * _impactReductionSpeed;
                _impact = Vector3.Lerp(startingImpact, Vector3.zero, percent);
                yield return null;
            }
            _impact = Vector3.zero;
            _impactReduction = null;
        }
        
    }
    
}
