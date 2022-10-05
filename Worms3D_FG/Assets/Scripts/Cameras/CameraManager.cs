using System;
using UnityEngine;
using Cinemachine;
using WormsGame.Core;
using WormsGame.Units;

namespace WormsGame.Cameras
{
    public class CameraManager : MonoBehaviour
    {
        [SerializeField] CinemachineVirtualCamera firstPersonCamera;
        [SerializeField] CinemachineVirtualCamera thirdPersonCamera;
        [SerializeField] GameObject _crossFire;
        Unit _currentUnit;

        void Awake()
        {
            FindObjectOfType<TurnHandler>().UnitChanged += FocusOnCurrentPlayer;
        }

        void Update()
        {
            ControlPerspective();
        }

        void ControlPerspective()
        {
            if (_currentUnit == null)
            {
                Debug.LogError("there is no current unit, report");
                this.gameObject.SetActive(false);
                return;
            }
            
            if (_currentUnit.InputHandler.IsAiming && !firstPersonCamera.enabled)
            {
                ChangeCamera(true);
            }
            else if(!_currentUnit.InputHandler.IsAiming && firstPersonCamera.enabled)
            {
                ChangeCamera(false);
            }
        }

        void ChangeCamera(bool setFirstPersonCamera)
        {
            if (setFirstPersonCamera)
            {
                _currentUnit.PlayerController.MatchCameras();
                
            }
            firstPersonCamera.enabled = setFirstPersonCamera;
            _crossFire.SetActive(setFirstPersonCamera);
            thirdPersonCamera.enabled = !setFirstPersonCamera;
            _currentUnit.PlayerController.SetRotateOnMove(firstPersonCamera.enabled);

        }

        public void FocusOnCurrentPlayer(Unit unit)
        {
            if (unit == null) return;
            firstPersonCamera.enabled = false;
            thirdPersonCamera.enabled = true;
            firstPersonCamera.Follow = unit.PlayerController.FirstPersonCamTarget;
            thirdPersonCamera.Follow = unit.PlayerController.ThirdPersonCamTarget;
            this._currentUnit = unit;
        }
    }
}