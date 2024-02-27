using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatDlg : MonoBehaviour
{
    [SerializeField] GameObject m_LoginUI;
    [SerializeField] GameObject m_prbUserItem;
    [SerializeField] Transform m_UserItemParent;
    [SerializeField] Button m_btnLogOut;
    [SerializeField] Button m_btnWithdraw;
    [SerializeField] InputField m_infSend;
    [SerializeField] Button m_btnSend;
    [SerializeField] GameObject m_prbTextItem;
    [SerializeField] Transform m_TextItemParent;
    [SerializeField] Scrollbar m_verticalBar;

    void Start()
    {
        SocketIoMgr.Inst.onNotify_UserInfo -= OnAckEvent_UserInfo;
        SocketIoMgr.Inst.onNotify_UserInfo += OnAckEvent_UserInfo;
        SocketIoMgr.Inst.onNotify_UserInfoList -= OnNotifyEvent_UserInfoList;
        SocketIoMgr.Inst.onNotify_UserInfoList += OnNotifyEvent_UserInfoList;
        SocketIoMgr.Inst.onAck_Logout -= OnAckEvent_Logout;
        SocketIoMgr.Inst.onAck_Logout += OnAckEvent_Logout;
        m_btnLogOut.onClick.AddListener(OnClicked_Logout);
        m_btnWithdraw.onClick.AddListener(OnClicked_Withdraw);
        m_infSend.onSubmit.AddListener(OnSubmit_Send);
        m_btnSend.onClick.AddListener(OnClicked_Send);
    }

    public void Create_UserItem(string id)
    {
        GameObject go = Instantiate(m_prbUserItem, m_UserItemParent);
        go.GetComponentInChildren<Text>().text = id;
    }


    void OnAckEvent_UserInfo(object obj, SocketIoMgr.UserInfo info)
    {
        Create_UserItem(info.id);
    }

    void OnNotifyEvent_UserInfoList(object obj, SocketIoMgr.UserInfoList list)
    {
        for (int i = 0; i < list.datas.Count; i++)
        {
            Create_UserItem(list.datas[i].id);
        }

        MyDataNet.Inst.StartServer(SocketIoMgr.Inst.m_MyUserInfo.ip, SocketIoMgr.Inst.m_MyUserInfo.dataPort);
    }

    void OnAckEvent_Logout(object obj, string id)
    {
        for (int i = 0; i < m_UserItemParent.childCount; i++)
        {
            Destroy(m_UserItemParent.GetChild(i).gameObject);
        }
        for (int i = 0; i < SocketIoMgr.Inst.m_UserInfoList.datas.Count; i++)
        {
            Create_UserItem(SocketIoMgr.Inst.m_UserInfoList.datas[i].id);
        }
        MyDataNet.Inst.CloseServer();
    }

    void OnClicked_Logout()
    {
        SocketIoMgr.Inst.SendLogout(SocketIoMgr.Inst.m_MyUserInfo.id);
        for (int i = 0; i < m_UserItemParent.childCount; i++)
        {
            Destroy(m_UserItemParent.GetChild(0).gameObject);
        }
        for (int i = 0; i < m_TextItemParent.childCount; i++)
        {
            Destroy(m_TextItemParent.GetChild(0).gameObject);
        }
        m_LoginUI.SetActive(true);
    }
    void OnClicked_Withdraw()
    {
        SocketIoMgr.Inst.SendWithdraw(SocketIoMgr.Inst.m_MyUserInfo.id);

        OnClicked_Logout();
    }
    void OnClicked_Send()
    {
        MyDataNet.Inst.SendBrodcastChatMsg(m_infSend.text);
        AddMessage(SocketIoMgr.Inst.m_MyUserInfo.id ,m_infSend.text);
        m_infSend.text = string.Empty;
    }
    void OnSubmit_Send(string s)
    {
        MyDataNet.Inst.SendBrodcastChatMsg(m_infSend.text);
        AddMessage(SocketIoMgr.Inst.m_MyUserInfo.id, m_infSend.text);
        m_infSend.text = string.Empty;
    }

    public void AddMessage(string id, string msg)
    {
        GameObject go = Instantiate(m_prbTextItem, m_TextItemParent);
        go.GetComponentInChildren<Text>().text = id + " : " + msg;
        m_verticalBar.value = 0;
    }
}
