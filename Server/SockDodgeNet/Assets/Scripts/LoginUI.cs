using socketionet;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginUI : MonoBehaviour
{
    [SerializeField] InputField m_infID;
    [SerializeField] InputField m_infPW;
    [SerializeField] Button m_btnLogin;
    [SerializeField] Button m_btnSignUp;
    [SerializeField] Button m_btnQuit;

    void Start()
    {
        m_btnLogin.onClick.AddListener(OnClicked_Login);
        m_btnSignUp.onClick.AddListener(OnClicked_SignUp);
        m_btnQuit.onClick.AddListener(OnClicked_Quit);
        CSocketIoMgr.Inst.onAck_Login -= OnAckEvent_Login;
        CSocketIoMgr.Inst.onAck_Login += OnAckEvent_Login;
        CSocketIoMgr.Inst.onAck_CreateAccount -= OnAckEvent_CreateId;
        CSocketIoMgr.Inst.onAck_CreateAccount += OnAckEvent_CreateId;
    }

    bool InfCheck()
    {
        if (m_infID != null && m_infPW != null)
            return true;
        return false;
    }
    void OnClicked_Login()
    {
        if(InfCheck())
            CSocketIoMgr.Inst.SendReqLogin(m_infID.text, m_infPW.text);
    }
    void OnClicked_SignUp()
    {
        if (InfCheck())
        {
            CSocketIoMgr.Inst.SendReqCreateId(m_infID.text, m_infPW.text);
            OnClicked_Login();
        }
    }
    void OnClicked_Quit()
    {

    }

    void OnAckEvent_Login(object sender, int success)
    {
        if (success == 0)
        {
            CSocketIoMgr.Inst.SendReqInitRoomList();
            gameObject.SetActive(false);
        }
    }
    void OnAckEvent_CreateId(object sender, int success)
    {

    }
}
