using System;
using System.Collections.Generic;
using System.Linq;
using _01.Script.Core.GameEventSystem;
using UnityEngine;

namespace _01.Script.Camera
{
    public class CameraCore : MonoBehaviour
    {
        [SerializeField] private GameEventChannelSO cameraEventChannel;

        private Dictionary<Type, GameCam> cameras;

        public GameCam CurrentCam { get; private set; }

        public void Awake()
        {
            ComponentSetting();

            cameraEventChannel.AddListener<CameraChangeEvent>(HandleCameraChange);
        }

        private void OnDestroy()
        {
            cameraEventChannel.RemoveListener<CameraChangeEvent>(HandleCameraChange);
        }

        private void HandleCameraChange(CameraChangeEvent evt)
        {
            Type type = evt.nextCam;

            var method = typeof(CameraCore).GetMethod("GetCamera");
            var genericMethod = method.MakeGenericMethod(type);
            GameCam cam = (GameCam)genericMethod.Invoke(this, null);

            ChangeCam(cam);
        }

        private void ComponentSetting()
        {
            cameras = new Dictionary<Type, GameCam>();

            GetComponentsInChildren<GameCam>().ToList().ForEach(camera =>
            {
                cameras.Add(camera.GetType(), camera);
            });

            cameras.Values.ToList().ForEach(camera =>
            {
                camera.Initialize(this);

                camera.SetPriority(-1);
            });

            ChangeCam(GetCamera<PlayerFollowCam>());
        }



        public T GetCamera<T>() where T : GameCam
        {
            return (T)cameras.GetValueOrDefault(typeof(T));
        }


        public void ChangeCam(GameCam nextCam)
        {
            if (CurrentCam != null)
                CurrentCam.SetPriority(-1);
            CurrentCam = nextCam;
            CurrentCam.SetPriority(0);
        }


    }
}