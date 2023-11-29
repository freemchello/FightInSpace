using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MVC.Views
{

    public class SpaceShipCreatorView : MonoBehaviour
    {

        [SerializeField] private Vector3 _rotationVectorVisualView;
        [SerializeField] private Transform ContainerPromoSpaceShip;

        [SerializeField] private GameObject _defaultSpaceShip;

        [SerializeField] private Transform ContainerSpaceShips;


        [SerializeField] private GameObject PanelMenu;
        [SerializeField] private GameObject PanelSpaceShipsShop;


        [SerializeField] private float _animationSpeedPanels;
        [SerializeField] private float _animationSpeedVisualView;
        private bool _isShopView;
        private GameObject _shipVisualView;


        public void OnPanelSpaceShipShop()
        {

            _isShopView = true;
        }

        public void OnPanelMenuView()
        {

            _isShopView = false;
        }


        private void Awake()
        {
            InstantiateDefaultSpaceShipVisual();
        }


        private void InstantiateDefaultSpaceShipVisual()
        {
            _shipVisualView = GameObject.Instantiate(_defaultSpaceShip, ContainerPromoSpaceShip);
            _shipVisualView.transform.localScale = new Vector3(5.6f, 5.6f, 5.6f);
        }


        public void ChangeSpaceShipVisualView(GameObject objectView)
        {

            var spaceShips = ContainerPromoSpaceShip.GetChild(0);

            Destroy(spaceShips.gameObject);

            _defaultSpaceShip = objectView;
            _shipVisualView = null;
            InstantiateDefaultSpaceShipVisual();
        }


        private void Update()
        {

            if (PanelMenu != null)
            {
                if (_isShopView)
                {
                    PanelMenu.transform.localScale = Vector3.zero;
                    PanelSpaceShipsShop.transform.localScale = Vector3.Lerp(PanelSpaceShipsShop.transform.localScale, Vector3.one, Time.deltaTime * _animationSpeedPanels);
                  
                }
                else
                {
                    PanelSpaceShipsShop.transform.localScale = Vector3.zero;
                    ContainerSpaceShips.localPosition = Vector3.zero;
                    PanelMenu.transform.localScale = Vector3.Lerp(PanelMenu.transform.localScale, Vector3.one, Time.deltaTime * _animationSpeedPanels);
                }
            }
            if(_shipVisualView != null)
            {
                _shipVisualView.transform.Rotate(_rotationVectorVisualView * Time.deltaTime * _animationSpeedVisualView);
            }
        }



    }
}