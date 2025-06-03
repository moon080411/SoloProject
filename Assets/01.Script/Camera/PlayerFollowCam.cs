using _01.Script.Core.GameEventSystem;
using _01.Script.SO;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _01.Script.Camera
{
    public class PlayerFollowCam : GameCam
    {

        [SerializeField] private CinemachinePositionComposer cam;

        [SerializeField] private GameEventChannelSO cameraEventChannel;
        
        private void Awake()
        {
            cameraEventChannel.AddListener<PlayerCamSelectEvent>(HandleSelectCam);
            cameraEventChannel.AddListener<PlayerCamUnSelectedEvent>(HandelUnSelectedCam);
        }

        private void OnDestroy()
        {
            cameraEventChannel.RemoveListener<PlayerCamSelectEvent>(HandleSelectCam);
            cameraEventChannel.RemoveListener<PlayerCamUnSelectedEvent>(HandelUnSelectedCam);
        }

        private void HandelUnSelectedCam(PlayerCamUnSelectedEvent evt)
        {
            
        }

        private void HandleSelectCam(PlayerCamSelectEvent evt)
        {
            
        }
    }
}
