using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NET_Player : MonoBehaviour
{

    public HashSet<string> ServerUsers = new();
     
    public static string LocalName;
    public static string LocalRoom;

    public static NET_Player SINGLETONE;

    private void Awake()
    {
        SINGLETONE = this;
    }
 
}
