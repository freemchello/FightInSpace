using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NET_Player : NetworkBehaviour
{
     
    public static List<AUTH_DATA> ServerData = new List<AUTH_DATA>();

   // public static Dictionary<NetworkConnectionToClient, AUTH_DATA> Players = new();

    public static string LocalName;
    public static string LocalRoom;

    //public static NET_Player LocalPlayer;


    //private void Awake()
    //{
    //    LocalPlayer = this;
    //}
 
    //public static void CmdNewUserConnect(string roomID, NetworkConnectionToClient conn)
    //{
    //    LocalPlayer.CmdSendAboutNewUser(conn);
    //}


    //[Command] 
    //private void CmdSendAboutNewUser(NetworkConnectionToClient conn)
    //{
    //    LocalPlayer.RpcSendAboutNewUser(conn);
    //}


    //[ClientRpc(includeOwner = true)]
    //private void RpcSendAboutNewUser(NetworkConnectionToClient conn)
    //{
    //    if (!Players.ContainsKey(conn))
    //    {

    //        Players.Add(conn, (AUTH_DATA)conn.authenticationData);
    //    }
    //    else
    //    {

    //    }
    //}
}
