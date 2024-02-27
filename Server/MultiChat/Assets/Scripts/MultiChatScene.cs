using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiChatScene : MonoBehaviour
{
    public ChatDlg m_ChatDlg;
    public LoginDlg m_LoginDlg;

    private void Awake()
    {
        MyDataNet.Inst.chatScene = this;
    }

    void Start()
    {
        SocketIoMgr.Inst.InitSocketIO();
    }

    void Update()
    {
        
    }

    private void OnApplicationQuit()
    {
        SocketIoMgr.Inst.SendLogout(SocketIoMgr.Inst.m_MyUserInfo.id);
    }
}
