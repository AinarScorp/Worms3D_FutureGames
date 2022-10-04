using System;
using UnityEngine;
using Cinemachine;
using WormsGame.Core;
using WormsGame.Movement;
using WormsGame.Units;

namespace WormsGame.Cameras
{
    public class CameraManager : MonoBehaviour
    {
        [SerializeField] CinemachineVirtualCamera firstPersonCamera;
        [SerializeField] CinemachineVirtualCamera thirdPersonCamera;
        [SerializeField] GameObject _crossFire;
        
        PlayerController _currentUnit;


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
                _currentUnit.MatchCameras();
                
            }
            firstPersonCamera.enabled = setFirstPersonCamera;
            _crossFire.SetActive(setFirstPersonCamera);
            thirdPersonCamera.enabled = !setFirstPersonCamera;
            _currentUnit.SetRotateOnMove(firstPersonCamera.enabled);

        }

        public void FocusOnCurrentPlayer(Unit unit)
        {
            unit.TryGetComponent(out PlayerController currentPlayer);
            if (currentPlayer == null) return;
            firstPersonCamera.enabled = false;
            thirdPersonCamera.enabled = true;
            firstPersonCamera.Follow = currentPlayer.FirstPersonCamTarget;
            thirdPersonCamera.Follow = currentPlayer.ThirdPersonCamTarget;
            this._currentUnit = currentPlayer;
        }
    }
}