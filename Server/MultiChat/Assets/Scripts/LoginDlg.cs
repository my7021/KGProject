using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginDlg : MonoBehaviour
{
    [SerializeField] InputField m_infID;
    [SerializeField] InputField m_infPass;
    [SerializeField] Button m_btnSignUp;
    [SerializeField] Button m_btnLogin;

    void Start()
    {
        m_btnSignUp.onClick.AddListener(OnClicked_SignUp);
        m_btnLogin.onClick.AddListener(OnClicked_Login);

        SocketIoMgr.Inst.InitSocketIO();

        SocketIoMgr.Inst.onAck_Join += OnAckEvent_Join;
        SocketIoMgr.Inst.onAck_Login += OnAckEvent_Login;
    }

    void OnClicked_SignUp()
    {
        if (infCheck())
        {
            SocketIoMgr.Inst.SendJoin(m_infID.text, m_infPass.text);
        }
    }

    void OnClicked_Login()
    {
        if(infCheck())
        {
            SocketIoMgr.Inst.SendLogin(m_infID.text, m_infPass.text);
        }
    }
    bool infCheck()
    {
        if (m_infID.text != null && m_infPass.text != null)
            return true;
        return false;
    }

    void OnAckEvent_Join(object sender, int success)
    {

    }

    void OnAckEvent_Login(object sender, int success)
    {
        if(success == 0)
        {
            gameObject.SetActive(false);
        }
    }
}
