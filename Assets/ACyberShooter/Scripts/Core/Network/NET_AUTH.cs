using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
namespace Networking
{

    public class NET_AUTH : NetworkAuthenticator
    {

        public GameObject _loginObjectUI;
        [SerializeField] private TMP_Text _errorText;
        private HashSet<string> _disconectedUsers = new();
        
        public string UserName;
        public string RoomID;


        private struct AuthResponse : NetworkMessage
        {
            public int ResponseCode;
            public string RoomID;
            public string UserName;
        }

        private struct AuthRequest : NetworkMessage
        {
            public string RoomID;
            public string UserName;
        }

        public struct DisconectRequest : NetworkMessage
        {
            
            public string UserName;
        }


        public void SetName(string name)
        {
            UserName = name;
        }

        public void SetRoom(string roomID)
        {
            RoomID = roomID;
        }

        public override void OnStartServer()
        {

            NetworkServer.RegisterHandler<AuthRequest>(OnRequest, false);
            NetworkServer.RegisterHandler<DisconectRequest>(OnClientDisconect, false);

        }

        public override void OnStopServer()
        {

            NetworkServer.UnregisterHandler<AuthRequest>();
            NetworkServer.UnregisterHandler<DisconectRequest>();

        }
        public override void OnStartClient() => NetworkClient.RegisterHandler<AuthResponse>(OnResponse, false);


        public override void OnStopClient() => NetworkClient.UnregisterHandler<AuthResponse>();


        public override void OnServerAuthenticate(NetworkConnectionToClient conn) { }



        public override void OnClientAuthenticate()
        {

            var request = new AuthRequest();

            request.UserName = UserName;
            request.RoomID = RoomID;

            NetworkClient.Send(request);
        }

          
        private void OnRequest(NetworkConnectionToClient conn, AuthRequest msg)
        {

                if (_disconectedUsers.Contains(msg.UserName))  return;
         
                if (NET_Player.SINGLETONE.ServerUsers.Contains(msg.UserName))
                {
                    conn.Send(new AuthResponse() { ResponseCode = 0, RoomID = msg.RoomID, UserName = msg.UserName });
                   
                }
                else
                {

                    CheckUserDatas(conn, msg);

                    var authentificator = new AUTH_DATA(msg.RoomID, msg.UserName);

                    conn.authenticationData = authentificator;

                    NET_Player.SINGLETONE.ServerUsers.Add(msg.UserName);

                    conn.Send(new AuthResponse() { ResponseCode = 100, RoomID = msg.RoomID, UserName = msg.UserName });
                    ServerAccept(conn);
                }

        }


        private void CheckUserDatas(NetworkConnectionToClient conn, AuthRequest msg)
        {

            if (RoomID == string.Empty)
            {
                conn.Send(new AuthResponse() { ResponseCode = 1, RoomID = msg.RoomID, UserName = msg.UserName });
            }
            if (UserName == string.Empty)
            {
                conn.Send(new AuthResponse() { ResponseCode = 2, RoomID = msg.RoomID, UserName = msg.UserName });
            }
        }


        private void OnClientDisconect(NetworkConnectionToClient conn, DisconectRequest msg)
        {

           
            DisconectConnection(conn, msg);
            Debug.Log($"Игрок {msg.UserName} отключен от сервера");
        }

        
        

        private void DisconectConnection(NetworkConnectionToClient conn, DisconectRequest msg)
        {
           
            _disconectedUsers.Remove(msg.UserName);
            NET_Player.SINGLETONE.ServerUsers.Remove(msg.UserName);
          
        }


        private void OnResponse(AuthResponse msg)
        {

            switch (msg.ResponseCode)
            {
                case 0:

                    _errorText.text = "Игрок с таким именем уже на сервере)";
                    
                    NET_MANAGER.SINGLETONE.StopHost();
                    NET_MANAGER.SINGLETONE.StopClient();
                    break;
                case 1:

                    _errorText.text = "Выберете локацию";

                    NET_MANAGER.SINGLETONE.StopHost();
                    NET_MANAGER.SINGLETONE.StopClient();
                    break;

                case 2:

                    _errorText.text = "Введите имя...";

                    NET_MANAGER.SINGLETONE.StopHost();
                    NET_MANAGER.SINGLETONE.StopClient();
                    break;

                case 100:

                    ClientAccept();
                    Debug.Log("Аунтификация пройдена! Доступ разрешен! ");

                    NET_MANAGER.SINGLETONE.USER_NAME = msg.UserName;

                    NET_Player.LocalName = msg.UserName;
                    NET_Player.LocalRoom = msg.RoomID;

                    _loginObjectUI.gameObject.SetActive(false);
                    NetworkClient.Send(new RequestOfCreatedCharacter());
                    break;

                default:

                    break;
            }
        }

       
    }
}