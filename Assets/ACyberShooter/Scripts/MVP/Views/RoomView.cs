using Networking;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MVC.Views
{

    public class RoomView : MonoBehaviour
    {

        [SerializeField] private NET_AUTH AUTHENTIFICATOR;

        [SerializeField] private TMP_Text _roomText;

        [SerializeField] private Button _room_01;
        [SerializeField] private Button _room_02;
        [SerializeField] private Button _room_03;
        [SerializeField] private Button _room_04;
        [SerializeField] private Button _room_05;
        [SerializeField] private Button _room_06;


        private void Awake()
        {


            _room_01.onClick.AddListener(() =>
            {
                string RoomID = "Битва за дрон";
                _roomText.text = RoomID;
                AUTHENTIFICATOR.RoomID = RoomID;
            });

            _room_02.onClick.AddListener(() =>
            {
                string RoomID = "Город грехов";
                _roomText.text = RoomID;
                AUTHENTIFICATOR.RoomID = RoomID;
            });

            _room_03.onClick.AddListener(() =>
            {
                string RoomID = "Выживший";
                _roomText.text = RoomID;
                AUTHENTIFICATOR.RoomID = RoomID;
            });

            _room_04.onClick.AddListener(() =>
            {
                string RoomID = "Вспышка черной дыры";
                _roomText.text = RoomID;
                AUTHENTIFICATOR.RoomID = RoomID;
            });

            _room_05.onClick.AddListener(() =>
            {
                string RoomID = "Сквозь горизонт";
                _roomText.text = RoomID;
                AUTHENTIFICATOR.RoomID = RoomID;
            });

            _room_06.onClick.AddListener(() =>
            {
                string RoomID = "Соколиный глаз";
                _roomText.text = RoomID;
                AUTHENTIFICATOR.RoomID = RoomID;
            });
        }

        private void OnDestroy()
        {
            _room_01.onClick.RemoveAllListeners();

            _room_02.onClick.RemoveAllListeners();

            _room_03.onClick.RemoveAllListeners();

            _room_04.onClick.RemoveAllListeners();

            _room_05.onClick.RemoveAllListeners();

            _room_06.onClick.RemoveAllListeners();
        }
    }
}