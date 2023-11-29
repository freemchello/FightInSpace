using UnityEngine;
using Abstracts;
using UnityEngine.InputSystem;
using UniRx.Triggers;
using UniRx;
using Cinemachine;
using System;
using System.Collections.Generic;

namespace Core
{

    public class SpaceShipCameraSystem : BaseSystem
    {
 
        private ISpaceShip _ship;

        private float _cameraLocked;
        private bool _isCameraLocked;

        private List<IDisposable> _disposables = new();


        protected override void Awake(IGameComponents components)
        {
 
            _ship = components.BaseObject.GetComponent<SpaceShipComponent>();
            _cameraLocked = 0;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

 
        protected override void FixedUpdate()
        {

            if (_ship.IsLockControll || _ship.PlayerInput == null) return;

            Vector3 delta = _ship.PlayerInput.Player.Mouse.ReadValue<Vector2>();
            float x_axis
                = delta.y *
                (Gamepad.all.Count > 0
                ?_ship.CameraConfig.Sensetivity_GamePad
                : _ship.CameraConfig.Sensetivity_Mouse) * 0.002f * Time.fixedDeltaTime;
            
            float y_axis = delta.x * (Gamepad.all.Count > 0
                ? _ship.CameraConfig.Sensetivity_GamePad
                : _ship.CameraConfig.Sensetivity_Mouse) * Time.fixedDeltaTime;

            _ship.Camera.m_YAxis.Value -= x_axis;
            _ship.Camera.m_XAxis.Value += y_axis;

        }


        protected override void Update()
        {

            if (_ship.PlayerInput.Player.Mouse.ReadValue<Vector2>().x == 0 && _ship.PlayerInput.Player.Mouse.ReadValue<Vector2>().y == 0 && !_isCameraLocked)
            {
                _cameraLocked += Time.deltaTime;
            }
            else
            {
                _cameraLocked = 0;
            }

            if (_cameraLocked >= 3f)
            {
                _isCameraLocked = true;
                _cameraLocked = 0;
                Observable.Timer(TimeSpan.FromSeconds(2)).Subscribe(_ => { _isCameraLocked = false; }).AddTo(_disposables);
            }

            if (_isCameraLocked)
            {
                _ship.Camera.m_YAxis.Value = Mathf.Lerp(_ship.Camera.m_YAxis.Value, 0.5f, Time.deltaTime * _ship.SpaceshipData.CameraTurnAmount);
                _ship.Camera.m_XAxis.Value = Mathf.Lerp(_ship.Camera.m_XAxis.Value, 0.0f, Time.deltaTime * _ship.SpaceshipData.CameraTurnAmount);
            }
        }
    }
}