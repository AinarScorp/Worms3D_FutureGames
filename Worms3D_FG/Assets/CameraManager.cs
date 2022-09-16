
using UnityEngine;
using Cinemachine;
using Main.Movement;

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
        if (currentPlayer.InputHandler.IsAiming)
        {
            print("yes");
            ChangeCamera(true);
        }
        else
        {
            print("no");

            ChangeCamera(false);
        }
    }

    void ChangeCamera(bool setFirstPersonCamera)
    {
        firstPersonCamera.enabled = setFirstPersonCamera;
        thirdPersonCamera.enabled = !setFirstPersonCamera;
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