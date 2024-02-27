using socketionet;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyScene : MonoBehaviour
{

    private void Awake()
    {
        if(!CSocketIoMgr.Inst.IsConnected)
            CSocketIoMgr.Inst.InitSocketIO("192.168.0.76", 19000);
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
