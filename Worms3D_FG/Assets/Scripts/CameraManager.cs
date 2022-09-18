using UnityEngine;
using Cinemachine;
using WormsGame.Movement;

namespace WormsGame.Cameras
{
    public class CameraManager : MonoBehaviour
    {
        [SerializeField] CinemachineVirtualCamera firstPersonCamera;
        [SerializeField] CinemachineVirtualCamera thirdPersonCamera;
        [SerializeField] PlayerController currentPlayer;


        void Start()
        {
            FocusOnCurrentPlayer(currentPlayer);
        }

        void Update()
        {
            ControlPerspective();
        }

        void ControlPerspective()
        {
            if (currentPlayer.InputHandler.IsAiming && !firstPersonCamera.enabled)
            {
                ChangeCamera(true);
            }
            else if(!currentPlayer.InputHandler.IsAiming && firstPersonCamera.enabled)
            {
                ChangeCamera(false);
            }
        }

        void ChangeCamera(bool setFirstPersonCamera)
        {
            firstPersonCamera.enabled = setFirstPersonCamera;
            thirdPersonCamera.enabled = !setFirstPersonCamera;
            currentPlayer.SetRotateOnMove(firstPersonCamera.enabled);

        }

        void FocusOnCurrentPlayer(PlayerController currentPlayer)
        {
            this.currentPlayer = currentPlayer;
            firstPersonCamera.enabled = false;
            thirdPersonCamera.enabled = true;
            firstPersonCamera.Follow = currentPlayer.CameraTarget;
            thirdPersonCamera.Follow = currentPlayer.CameraTarget;
        }
    }
}