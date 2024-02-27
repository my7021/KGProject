using socketionet;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour
{
    [SerializeField] Text m_txtUserID;
    [SerializeField] InputField m_infRoomName;
    [SerializeField] Button m_btnJoin;
    [SerializeField] Button m_btnCreate;
    [SerializeField] Button m_btnOption;

    public RoomListUI m_RoomListUI = null;


    void Start()
    {
        m_btnJoin.onClick.AddListener(OnClicked_Join);
        m_btnCreate.onClick.AddListener(OnClicked_Create);
        m_btnOption.onClick.AddListener(OnClicked_Option);
    }

    void OnClicked_Join()
    {
        if(m_infRoomName.text != null)
        {
            CSocketIoMgr.Inst.SendReqJoinRoom(m_infRoomName.text);
        }
    }
    void OnClicked_Create()
    {
        if (m_infRoomName.text != null)
        {
            CSocketIoMgr.Inst.SendReqCreateRoom(m_infRoomName.text);
        }
    }
    void OnClicked_Option()
    {

    }

    
}
