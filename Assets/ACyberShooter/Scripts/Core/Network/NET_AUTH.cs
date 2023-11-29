using Mirror;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace Networking
{

    public class NET_AUTH : NetworkAuthenticator
    {

        [SerializeField] private GameObject _loginObjectUI;

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


        public void SetName(string name)
        {
            UserName = name;
        }

        public void SetRoom(string roomID)
        {
            RoomID = roomID;
        }

        public override void OnStartServer() => NetworkServer.RegisterHandler<AuthRequest>(OnRequest, false);


        public override void OnStopServer() => NetworkServer.UnregisterHandler<AuthRequest>();


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

            if (_disconectedUsers.Contains(msg.UserName))
            {
                return;
            }
            else
            {
                var varriableAuth = NET_Player.ServerData.Where(data => data.ConnectionClient == conn).FirstOrDefault();

                if (varriableAuth != null)
                {
                    conn.Send(new AuthResponse() { ResponseCode = 0, RoomID = msg.RoomID, UserName = msg.UserName });
                    ServerReject(conn);
                }
                else
                {
                    var authentificator = new AUTH_DATA(msg.RoomID, msg.UserName);

                    authentificator.ConnectionClient = conn;
                    conn.authenticationData = authentificator;

                    NET_Player.ServerData.Add(authentificator);
                   // NET_Player.CmdNewUserConnect(msg.RoomID, conn);
                    conn.Send(new AuthResponse() { ResponseCode = 100, RoomID = msg.RoomID, UserName = msg.UserName });
                    ServerAccept(conn);
                }
            }
        }

        private void OnResponse(AuthResponse msg)
        {

            switch (msg.ResponseCode)
            {
                case 0:
                    Debug.LogError("Сервер не принял! Аунтификация не пройдена");
                    ClientReject();
                    break;

                case 100:

                    ClientAccept();
                    Debug.Log("Аунтификация пройдена!");

                    NET_Player.LocalName = msg.UserName;
                    NET_Player.LocalRoom = msg.RoomID;
                    _loginObjectUI.gameObject.SetActive(false);
                    
                    break;

                default:

                    break;
            }
        }

       
    }
}