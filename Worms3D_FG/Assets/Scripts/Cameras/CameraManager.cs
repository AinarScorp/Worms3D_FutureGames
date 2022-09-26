using UnityEngine;
using Cinemachine;
using WormsGame.Movement;

namespace WormsGame.Cameras
{
    public class CameraManager : MonoBehaviour
    {
        [SerializeField] CinemachineVirtualCamera firstPersonCamera;
        [SerializeField] CinemachineVirtualCamera thirdPersonCamera;
        [SerializeField] PlayerController currentUnit;

        [SerializeField] GameObject _crossFire;
        void Update()
        {
            ControlPerspective();
        }

        void ControlPerspective()
        {
            if (currentUnit == null)
            {
                this.gameObject.SetActive(false);
                return;
            }
            
            if (currentUnit.InputHandler.IsAiming && !firstPersonCamera.enabled)
            {
                ChangeCamera(true);
            }
            else if(!currentUnit.InputHandler.IsAiming && firstPersonCamera.enabled)
            {
                ChangeCamera(false);
            }
        }

        void ChangeCamera(bool setFirstPersonCamera)
        {
            if (setFirstPersonCamera)
            {
                currentUnit.MatchCameras();
                
            }
            firstPersonCamera.enabled = setFirstPersonCamera;
            _crossFire.SetActive(setFirstPersonCamera);
            thirdPersonCamera.enabled = !setFirstPersonCamera;
            currentUnit.SetRotateOnMove(firstPersonCamera.enabled);

        }

        public void FocusOnCurrentPlayer(GameObject playerController)
        {
            playerController.TryGetComponent(out PlayerController currentPlayer);
            if (currentPlayer == null) return;
            firstPersonCamera.enabled = false;
            thirdPersonCamera.enabled = true;
            firstPersonCamera.Follow = currentPlayer.FirstPersonCamTarget;
            thirdPersonCamera.Follow = currentPlayer.ThirdPersonCamTarget;
            this.currentUnit = currentPlayer;
        }
    }
}