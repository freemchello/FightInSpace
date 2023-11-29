using System.Collections.Generic;
using Core;
using UnityEngine;
using Mirror;
using TMPro;

namespace Abstracts
{
    
    public abstract class StateMachine : NetworkBehaviour
    {

        [SerializeField] private Camera _camera;
        [SerializeField] private List<GameObject> _noneLocalPlayerHiden = new();
        [SerializeField] private TMP_Text _nameText;

        [SyncVar] private string _name;

          
        private List<ISystem> _systems;

        protected abstract List<ISystem> GetSystems();

        PlayerInput input;


       [Command(requiresAuthority = false)]
        private void CmdSetName(NetworkConnectionToClient conn)
        {
            var auth = (AUTH_DATA) conn.authenticationData;
            Debug.Log(auth.UserName);
            _name = auth.UserName;
            _nameText.text = auth.UserName;
        }

        //public override void OnStartClient()
        //{
        //    // base.OnStartClient();
        //    if(!isLocalPlayer) return;
        //    input = new PlayerInput();

        //    input.Enable();
        //    Init();
        //    CmdSetName(connectionToClient);
             
        //    _nameText.gameObject.SetActive(false);

        //}
        public override void OnStartLocalPlayer()
        {
            // base.OnStartLocalPlayer();
            input = new PlayerInput();

            //    input.Enable();
            Init();
            CmdSetName(connectionToClient);
            Debug.Log(_name);

            _nameText.gameObject.SetActive(false);
        }

        private void Init()
        {
            
            _systems = GetSystems();

            ObjectStack stack = new ObjectStack(_camera, this.gameObject);
            
            for (int i = 0; i < _systems.Count; i++)
            {
                _systems[i].BaseAwake(stack);
            }

            for (int i = 0; i < _systems.Count; i++)
            {
                _systems[i].BaseStart();
            }

            Debug.Log("Инициализация пройдена!");
        }
        
        
        private void OnEnable()
        {
            if (!isLocalPlayer) return;

            for (int i = 0; i < _systems.Count; i++)
            {
                _systems[i].BaseOnEnable();
            }
        }
        
        
        private void Start()
        {

            if (!isLocalPlayer)
            {

                //var auth = (AUTH_DATA) connectionToClient.authenticationData;
                //Debug.Log(auth.UserName + "  Получение аунтификационных данных настоящим клиентом = " + _name);

                //if (NET_Player.LocalRoom != auth.RoomID)
                //{
                //    gameObject.SetActive(false);
                //    return;
                //}
                _camera.gameObject.SetActive(false);
                _noneLocalPlayerHiden.ForEach(h => h.gameObject.SetActive(false));


                _nameText.color = Color.blue;

                return;
            }
            else
            {
                input.Enable();
            }

           
        }
        
        
        private void Update()
        {

            if (!isLocalPlayer)
            {
               // var auth = (AUTH_DATA)connectionToClient.authenticationData;
               // Debug.Log(auth.UserName + "  Получение аунтификационных данных настоящим клиентом = " + _name);
               // _nameText.gameObject.transform.LookAt(this.gameObject.transform);
                
                return;
            }

            for (int i = 0; i < _systems.Count; i++)
            {
                _systems[i].BaseUpdate();
            }
        }
        
        
        private void FixedUpdate()
        {

            if (!isLocalPlayer) return;

            for (int i = 0; i < _systems.Count; i++)
            {
                _systems[i].BaseFixedUpdate();
            }
        }
        
        
        private void LateUpdate()
        {

            if (!isLocalPlayer) return;

            for (int i = 0; i < _systems.Count; i++)
            {
                _systems[i].BaseLateUpdate();
            }
        }


        private void OnDisable()
        {

            if (!isLocalPlayer) return;

            for (int i = 0; i < _systems.Count; i++)
            {
                _systems[i].BaseOnDisable();
            }
        }
        
        
    }
}
