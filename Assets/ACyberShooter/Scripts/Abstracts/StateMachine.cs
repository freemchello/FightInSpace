using System.Collections.Generic;
using Core;
using UnityEngine;
using Mirror;
using TMPro;
using Networking;

namespace Abstracts
{
    
    public abstract class StateMachine : NetworkBehaviour
    {

        [SerializeField] private Camera _camera;
        [SerializeField] private List<GameObject> _noneLocalPlayerHiden = new();
        [SerializeField] private TMP_Text _nameText;
       
        private string _name;

          
        private List<ISystem> _systems;

        protected abstract List<ISystem> GetSystems();

        
        [Command(requiresAuthority = false)]
        private void CmdSetName(NetworkConnectionToClient conn)
        {
             
            _nameText.text = _name;
        }
 
        public override void OnStartLocalPlayer()
        {
              
            Init();
            _name = NET_MANAGER.SINGLETONE.USER_NAME;
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

            if (Input.GetKeyDown(KeyCode.Escape))
            {
               
                NET_MANAGER.SINGLETONE.Disconect();

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
