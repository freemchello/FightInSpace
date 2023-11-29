using Mirror;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static Networking.NET_AUTH;


namespace Networking
{

    public struct RequestOfCreatedCharacter : NetworkMessage
    {

        public string test;
    }


    public class NET_MANAGER : NetworkManager
    {

        [SerializeField] private GameObject UI_Login;

        private Dictionary<string, GameObject> _userShips = new();

        public static NET_MANAGER SINGLETONE;
        public string USER_NAME;
         

        public override void Awake()
        {
            base.Awake();

            SINGLETONE = this;
        }


        public void Disconect()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            UI_Login.gameObject.SetActive(true);

            StopClient();
        }

        public override void OnStartServer()
        {

            NetworkServer.RegisterHandler<RequestOfCreatedCharacter>(OnCreatedCharacterRequest, false);
        }

        public override void OnServerDisconnect(NetworkConnectionToClient conn)
        {

            AUTH_DATA data = (AUTH_DATA) conn.authenticationData;
           
            NetworkClient.Send(new DisconectRequest() { UserName = data.UserName });

            NetworkServer.Destroy(_userShips[data.UserName]);
           // base.OnServerDisconnect(conn);

            _userShips.Remove(data.UserName);
        }   
       

        public override void OnStopServer()
        {

            NetworkServer.UnregisterHandler<RequestOfCreatedCharacter>();
        }
          
         
        private void OnCreatedCharacterRequest(NetworkConnectionToClient conn, RequestOfCreatedCharacter msg)
        {

            AUTH_DATA data = (AUTH_DATA)conn.authenticationData;
            GameObject character = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
            NetworkServer.AddPlayerForConnection(conn, character);
            _userShips.Add(data.UserName, character);
            Debug.Log("Created space ship");
        }
    }
}