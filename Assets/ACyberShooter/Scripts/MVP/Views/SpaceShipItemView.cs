using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MVC.Views
{

    public class SpaceShipItemView : MonoBehaviour
    {

        public GameObject SpaceShipFabVisual;

        public Button _button;

        [SerializeField] private Vector3 _rotationVector = new Vector3(-.5f, 1f ,0f);

        [SerializeField] private Transform _rotationObject;

        [SerializeField] private float _animationRotationSpeed = 30f;


        public void InitView()
        {
            
            _button = GetComponent<Button>();

            SpaceShipFabVisual = transform.GetChild(0).transform.GetChild(0).gameObject;

            _rotationObject = transform.GetChild(0);


        }


        private void Update()
        {

            if(_rotationObject != null)
            {

                _rotationObject.Rotate(_rotationVector * _animationRotationSpeed * Time.deltaTime);
            }
        }
    }
}