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
        [SerializeField] private POVInputSO povInput;
        [SerializeField] private CamSettingSO camSetting;

        [SerializeField] private GameEventChannelSO cameraEventChannel;

        private float _turnSpeedX;
        private float _turnSpeedY;

        private Vector2 _xMinMax;
        private Vector2 _camDistanceMinMax;

        private bool _canRotate;
        private Vector2 _lockCursorPos = Vector2.zero;

        public bool isCanZoomInOut = true;
        public bool isCanCameraMoving = true;



        private void Awake()
        {
            CamSettingSOChanged();
            povInput.LookedChange += Looked;
            povInput.PointerDeltaChanged += LookChange;
            povInput.ZoomedChange += ZoomInOut;

            cameraEventChannel.AddListener<PlayerCamSelectEvent>(HandleSelectCam);
            cameraEventChannel.AddListener<PlayerCamUnSelectedEvent>(HandelUnSelectedCam);
        }

        private void OnDestroy()
        {
            povInput.LookedChange -= Looked;
            povInput.PointerDeltaChanged -= LookChange;
            povInput.ZoomedChange -= ZoomInOut;

            cameraEventChannel.RemoveListener<PlayerCamSelectEvent>(HandleSelectCam);
            cameraEventChannel.RemoveListener<PlayerCamUnSelectedEvent>(HandelUnSelectedCam);
        }

        private void HandelUnSelectedCam(PlayerCamUnSelectedEvent evt)
        {
            isCanZoomInOut = false;
            isCanCameraMoving = false;
        }

        private void HandleSelectCam(PlayerCamSelectEvent evt)
        {
            isCanZoomInOut = true;
            isCanCameraMoving = true;
        }

        private void ZoomInOut(float value)
        {
            if (isCanZoomInOut == false) return;
            cam.CameraDistance = Mathf.Clamp(cam.CameraDistance - value, _camDistanceMinMax.x, _camDistanceMinMax.y);
        }

        // private void Update()
        // {
        //     GoDefault();
        // }

        // private void GoDefault()
        // {
        //     if (_isRotateChange)
        //     {
        //         _timer -= Time.deltaTime;
        //         if (_timer <= 0)
        //         {
        //             _isRotateChange = false;
        //             _isRotateStart = true;
        //             _timer = 0;
        //         }
        //     }
        //     else if (_isRotateStart)
        //     {
        //         _timer += Time.deltaTime;
        //         transform.localRotation = Quaternion.Lerp(transform.localRotation , _camDefaultRotate, _timer * _goDefaultRotateTime);
        //         if (_timer * _goDefaultRotateTime >= 1)
        //         {
        //             _isRotateStart = false;
        //             transform.localRotation = _camDefaultRotate;
        //         }
        //     }
        // }

        private void CamSettingSOChanged()
        {
            _turnSpeedX = camSetting.TurnSpeedX;
            _turnSpeedY = camSetting.TurnSpeedY;

            _xMinMax = camSetting.XMinMax;
            _camDistanceMinMax = camSetting.CamDistanceMinMax;

            //_camDefaultRotate = Quaternion.Euler(camSetting.CamDefaultRotate);
        }

        public void Looked(bool canRotate)
        {
            _canRotate = canRotate;
            if (canRotate)
            {
                _lockCursorPos = povInput.PointerPos;
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Mouse.current.WarpCursorPosition(_lockCursorPos);
            }
        }

        public void LookChange(Vector2 delta)
        {
            if (_canRotate == false || isCanCameraMoving == false)
                return;

            float rotationX = -delta.y * _turnSpeedY * Time.fixedDeltaTime;
            float rotationY = delta.x * _turnSpeedX * Time.fixedDeltaTime;

            Vector3 changedEuler = transform.eulerAngles;

            if (Mathf.Abs(changedEuler.x) > 180f) changedEuler.x -= 360f;

            changedEuler.x = Mathf.Clamp(changedEuler.x + rotationX, _xMinMax.x, _xMinMax.y);
            changedEuler.y += rotationY;

            transform.eulerAngles = changedEuler;
        }
    }
}
