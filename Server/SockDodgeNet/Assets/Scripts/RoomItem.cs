using socketionet;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomItem : MonoBehaviour
{
    public Text m_roomName;
    public Text m_roomState;
    public void Initialize(SORoom room)
    {
        m_roomName.text = $"{room.roomId} ( {room.players.Count}/{room.maxPlayer} )";
        m_roomState.text = (room.roomState == 0) ? "대기":"게임";
    }
}
