using MVC.Views;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ShopView : MonoBehaviour
{

    [SerializeField] private Transform ConteinerSpaceShips;

    private List<SpaceShipItemView> _itemViewsFabs = new();

    [SerializeField] private List<GameObject> _visualFabs;

    [SerializeField] private GameObject _defaultVisualFab;
    [SerializeField]  private GameObject _visualFabDecorator;

    [SerializeField] private Transform _promoTransform;

    [SerializeField] private Vector3 _rotationVector;

    [SerializeField] private Transform _rotationObject;

    [SerializeField] private float _animationRotateSpeed;


    private void Awake()
    {

        foreach(var obj in _visualFabs)
        {

            var decorator = Instantiate(_visualFabDecorator, ConteinerSpaceShips);

            var visualObj = Instantiate(obj, decorator.transform.GetChild(0));
            var itemView = decorator.GetOrAddComponent<SpaceShipItemView>();
            _itemViewsFabs.Add(itemView);

            itemView.InitView();
        }

        _itemViewsFabs.ForEach(item =>
        {
            item._button.onClick.AddListener( ()  => OnButtonItemClicked(item) );
            
        });

        var transformVisual = _promoTransform.GetChild(0);

        if (transformVisual != null)
        {
            Destroy(transformVisual.gameObject);

        }
        Instantiate(_defaultVisualFab, _promoTransform);

        _itemViewsFabs.ForEach(item =>
        {
            item._button.GetComponent<Image>().color = Color.black;

        });
        _itemViewsFabs[0]._button.GetComponent<Image>().color = Color.white;
    }

    private void Update()
    {

        if(_rotationObject != null)
        {
            _rotationObject.Rotate(_rotationVector * Time.deltaTime * _animationRotateSpeed);
        }
    }

    private void OnButtonItemClicked(SpaceShipItemView view)
    {

        _itemViewsFabs.ForEach(item =>
        {
            item._button.GetComponent<Image>().color = Color.black;

        });

        view._button.GetComponent<Image>().color = Color.white;

        var transformVisual =  _promoTransform.GetChild(0);

        if (transformVisual != null)
        {
            Destroy(transformVisual.gameObject);

        }
        Instantiate(view.SpaceShipFabVisual, _promoTransform);

    }

    private void OnDestroy()
    {
        _itemViewsFabs.ForEach(item =>
        {
            item._button.onClick.RemoveAllListeners();

        });
    }
}
