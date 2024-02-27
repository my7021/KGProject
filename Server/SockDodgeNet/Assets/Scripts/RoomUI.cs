using socketionet;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomUI : MonoBehaviour
{
    [SerializeField] Text m_txtRoomName;
    [SerializeField] Text m_txtNumPlayer;
    [SerializeField] Button m_btnReady;
    [SerializeField] Button m_btnStart;
    [SerializeField] Button m_btnExit;

    public List<PlayerItem> m_PlayerList = null;

    void Awake()
    {
        CSocketIoMgr.Inst.onNotify_EnterRoom -= OnNotify_EnterRoom;
        CSocketIoMgr.Inst.onNotify_EnterRoom += OnNotify_EnterRoom;
        CSocketIoMgr.Inst.onNotify_LeaveRoom -= OnNotify_LeaveRoom;
        CSocketIoMgr.Inst.onNotify_LeaveRoom += OnNotify_LeaveRoom;
        CSocketIoMgr.Inst.onAck_RoomUserReady -= OnAck_RoomUserReady;
        CSocketIoMgr.Inst.onAck_RoomUserReady += OnAck_RoomUserReady;
    }

    void Start()
    {
        m_txtRoomName.text = $"Room : {CSocketIoMgr.MyRoom.roomId}";
        m_txtNumPlayer.text = $"Player : {CSocketIoMgr.MyRoom.PlayerCount()} / {CSocketIoMgr.MyRoom.maxPlayer}";
        UpdatePlayer();
    }

    void OnClicked_Ready()
    {

    }
    void OnClicked_Start()
    {

    }
    void OnClicked_Exit()
    {
        CSocketIoMgr.Inst.SendReqLeaveRoom(CSocketIoMgr.MyUserInfo.id);
    }

    public void UpdatePlayer()
    {
        for (int i = 0; i < m_PlayerList.Count; i++)
        {
            m_PlayerList[i].ClearItem();
        }
        for (int i = 0; i < CSocketIoMgr.MyRoom.PlayerCount(); i++)
        {
            m_PlayerList[i].Initialize(CSocketIoMgr.MyRoom.players[i]);
        }
    }
    public void UpdateUserState(SORoomUserState state)
    {
        for (int i = 0; i < m_PlayerList.Count; i++)
        {
            PlayerItem player = m_PlayerList.Find((x) => x.name == state.id);
            if(player != null)
            {

            }
        }
    }


    void OnNotify_EnterRoom(object obj, SOPlayer player)
    {
        UpdatePlayer();
    }
    void OnNotify_LeaveRoom(object obj, string playerId)
    {
        UpdatePlayer();
    }
    void OnAck_RoomUserReady(object obj, SORoomUserState state)
    {
        UpdateUserState(state);
    }
}
