using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOptionModel : MonoBehaviour
{
    [SerializeField] Vector3 _rotationAngle;
    [SerializeField] float _speedRoation;
    bool _turningLeft = false;


    void OnEnable()
    {
        StartCoroutine(Turning(EndRotation()));
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }

    Quaternion EndRotation()
    {
        _turningLeft = !_turningLeft;
        return _turningLeft ? Quaternion.Euler(-_rotationAngle.x, -_rotationAngle.y, -_rotationAngle.z) : 
            Quaternion.Euler(_rotationAngle.x, _rotationAngle.y, _rotationAngle.z);
    }
    IEnumerator Turning(Quaternion endRotation)
    {
        Quaternion startRot = transform.rotation;
        Quaternion endRot = endRotation;
        float percent = 0;
        while (percent<1)
        {
            percent += Time.deltaTime *_speedRoation;
            transform.rotation = Quaternion.Lerp(startRot, endRot, percent);
            yield return null;
        }

        yield return Turning(EndRotation());
    }
}
