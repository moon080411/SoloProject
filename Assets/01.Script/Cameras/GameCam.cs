using Unity.Cinemachine;
using UnityEngine;

namespace _01.Script.Cameras
{
    public abstract class GameCam : MonoBehaviour
    {
        [SerializeField] protected CinemachineCamera myCam;

        private CameraCore _cameraCore;

        public virtual void Initialize(CameraCore cameraCore) => _cameraCore = cameraCore;


        public virtual void SetPriority(int settingValue = 0)
        {
            myCam.Priority = settingValue;
        }

        public virtual void SetLensSize(float size = 60)
        {
            myCam.Lens.FieldOfView = size;
        }

        public virtual void SetFollowTarget(Transform target)
        {
            myCam.Target.TrackingTarget = target;
        }
    }
}