using socketionet;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RoomListUI : MonoBehaviour
{
    [SerializeField] Text m_txtUserID;
    [SerializeField] GameObject m_prbRoomItem;
    [SerializeField] Transform m_RoomItemParent;

    public SORoomList m_roomList = null;

    void Start()
    {
        CSocketIoMgr.Inst.onAck_InitRoomList -= OnAck_init_roomlist;
        CSocketIoMgr.Inst.onAck_InitRoomList += OnAck_init_roomlist;
        CSocketIoMgr.Inst.onAck_CreateRoom -= OnAck_CreateRoom;
        CSocketIoMgr.Inst.onAck_CreateRoom += OnAck_CreateRoom;
        CSocketIoMgr.Inst.onAck_JoinRoom -= OnAck_JoinRoom;
        CSocketIoMgr.Inst.onAck_JoinRoom += OnAck_JoinRoom;
        CSocketIoMgr.Inst.onNotify_UpdateRoomList -= OnNotify_update_roomlist;
        CSocketIoMgr.Inst.onNotify_UpdateRoomList += OnNotify_update_roomlist;
    }

    public void CreateItem(SORoom room)
    {
        GameObject go = Instantiate(m_prbRoomItem, m_RoomItemParent);
        go.GetComponent<RoomItem>().Initialize(room);
    }

    public void InitRoomList(SORoomList roomList)
    {
        ClearRoomList();
        for (int i = 0; i < roomList.datas.Count; i++)
        {
            CreateItem(roomList.datas[i]);
        }
    }
    public void ClearRoomList()
    {
        for (int i = 0; i < m_RoomItemParent.childCount; i++)
        {
            Destroy(m_RoomItemParent.GetChild(0));
        }
    }
    void UpdateRoom()
    {
            for (int i = 0; i < m_roomList.datas.Count; i++)
            {
                m_RoomItemParent.GetChild(i).GetComponent<RoomItem>().Initialize(m_roomList.datas[i]);
            }
    }
    void OnAck_init_roomlist(object obj, SORoomList roomList)
    {
        m_txtUserID.text = CSocketIoMgr.NickName();
        m_roomList = roomList;
        InitRoomList(roomList);
    }
    void OnAck_CreateRoom(object obj, SORoom room)
    {
        m_roomList.datas.Add(room);
        CreateItem(room);
        SceneManager.LoadScene(1);
    }
    void OnAck_JoinRoom(object obj, SORoom room)
    {
        int success = (int)obj;
        if(success == 0)
            SceneManager.LoadScene(1);
    }
    void OnNotify_update_roomlist(object obj, SORoomList roomList)
    {
        m_roomList = roomList;
        InitRoomList(roomList);
    }
}
