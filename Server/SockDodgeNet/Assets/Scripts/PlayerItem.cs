using socketionet;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerItem : MonoBehaviour
{
    [SerializeField] GameObject m_checkMine;
    [SerializeField] GameObject m_checkMaster;
    [SerializeField] Text m_txtState;
    [SerializeField] Text m_txtPlayerName;  

    string m_name = string.Empty;

    void Start()
    {
        
    }

    public void Initialize(SOPlayer player)
    {
        if (player.userState == 0)
        {
            ClearItem();
            return;
        }
        m_name = player.Name();
        m_checkMine.SetActive(player.userInfo.IsMine(CSocketIoMgr.MyUserInfo.id));
        m_checkMaster.SetActive(player.isMaster);
        m_txtState.text = (player.userState == 2) ? "Ready" : "Enter";
        m_txtPlayerName.text = player.Name();
    }
    public void ClearItem()
    {
        m_checkMine.SetActive(false);
        m_checkMaster.SetActive(false);
        m_txtState.text = "Empty";
        m_txtPlayerName.text = "None";
    }
}
