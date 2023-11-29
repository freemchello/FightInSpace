using Abstracts;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;



namespace Core {


    public class SpaceShipMovableSystem : BaseSystem
    {

        private IGameComponents _components;

        private ISpaceShip _ship;
        
        private Vector3 rawInputSteering;
        private Vector3 smoothInputSteering;

        public Transform projectileSpawnTransform;
        private float nextShot = 0.0f;
        //Thrust

        private float rawInputThrust;
        private float smoothInputThrust;

        //Shooting
        private bool shootHeld;

        private float _currentAcceleration;
 

        protected override void Awake(IGameComponents components)
        {

            _components = components;
         
            _ship = components.BaseObject.GetComponent<SpaceShipComponent>();

            _ship.IsLockControll = false;
            _ship.PlayerInput = new PlayerInput();

            _ship.PlayerInput.Enable();
            
        }
  

        protected override void FixedUpdate()
        {
            
         //   if (_ship.IsLockControll || _ship.PlayerInput == null) return;
            
            MoveSpaceship();
            TurnSpaceship();
            CalculateShootingLogic();
        }


        private void MoveSpaceship()
        {

            _currentAcceleration = _ship.PlayerInput.SpaceShip.Acceleration.ReadValue<float>() * _ship.SpaceshipData.thrustAmountSprint;
            _currentAcceleration = (_currentAcceleration == 0 ? 1 : _currentAcceleration);
            _ship.Rigidbody.velocity
                = _ship.Rigidbody.transform.forward
                * _ship.SpaceshipData.thrustAmount * (_currentAcceleration == 0 ? 1 : _currentAcceleration)
                * (Mathf.Max(_ship.SpaceshipData.thrustInput, .2f));
        }


        private void TurnSpaceship()
        {

            float rollValue = 0;
            Vector2 rolling = Vector2.zero;
            rolling.x = _ship.PlayerInput.SpaceShip.Roll_Right.ReadValue<float>() == 1 ? 1 : 0;
            rolling.y = _ship.PlayerInput.SpaceShip.Roll_Left.ReadValue<float>() == 1 ? 1 : 0;

            if (rolling.x > 0)
            {
                rollValue = 1.0f;
            }
            else if (rolling.y > 0)
            {
                rollValue = -1.0f;
            }
            else
            {
                rollValue = 0.0f;
            }

            Vector3 newTorque
                = new Vector3(_ship.SpaceshipData.steeringInput.x * _ship.SpaceshipData.pitchSpeed * 500,
                -_ship.SpaceshipData.steeringInput.z * _ship.SpaceshipData.yawSpeed * 500,
                -_ship.SpaceshipData.rollSpeed * rollValue * 500f
            );

            _ship.Rigidbody.AddRelativeTorque(newTorque);
             
            VisualSpaceshipTurn();
        }


        private void VisualSpaceshipTurn()
        {

            _ship.VisualModel.localEulerAngles = new Vector3(_ship.SpaceshipData.steeringInput.x * _ship.SpaceshipData.leanAmount_Y
                , _ship.VisualModel.localEulerAngles.y, _ship.SpaceshipData.steeringInput.z * _ship.SpaceshipData.leanAmount_X);
        }


        private void CalculateShootingLogic()
        {

            if (_ship.SpaceshipData.shootInput == true)
            {
                if (Time.time > nextShot)
                {
                    ShootProjectile();
                    nextShot = Time.time + _ship.SpaceshipData.shootRate;
                }
            }
        }


        private void ShootProjectile()
        {

            GameObject newProjectile = GameObject.Instantiate(_ship.DefaultProjectTile);
            newProjectile.transform.position = _ship.ProjectTileSpawnTransform.position;
            newProjectile.transform.rotation = _ship.ProjectTileSpawnTransform.rotation;
            newProjectile.SetActive(true);
            newProjectile.GetComponent<Rigidbody>()
                .AddForce(_ship.Rigidbody.velocity + newProjectile.transform.forward * _ship.DefaultProjectTileSpeed, ForceMode.Impulse);
            GameObject.Destroy(newProjectile, 10);
        }


        protected override void Update()
        {

            if (_ship.IsLockControll || _ship.PlayerInput == null) return;
            SetInputData();


            if (_ship.PlayerInput.SpaceShip.Shoot.IsPressed())
            {
                shootHeld = true;
            }
            else
            {
                shootHeld = false;
            }

            Vector2 rawInput = _ship.PlayerInput.Player.Move.ReadValue<Vector2>();
            rawInputSteering = new Vector3(rawInput.y, 0, -rawInput.x);

            InputSmoothing();

          
        }


        private void InputSmoothing()
        {
            //Steering
            smoothInputSteering = Vector3.Lerp(smoothInputSteering, rawInputSteering, Time.deltaTime * _ship.SpaceshipData.SteeringSmoothing);

            //Thrust
            smoothInputThrust = Mathf.Lerp(smoothInputThrust, rawInputThrust, Time.deltaTime * _ship.SpaceshipData.ThrustSmoothing);
        }


        private void SetInputData() => _ship.SpaceshipData.UpdateInputData(smoothInputSteering, smoothInputThrust, shootHeld);
         
    }
}