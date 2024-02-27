using UnityEngine;
using UnityEngine.UI;

public class ChatDlg2 : MonoBehaviour
{
    [Header("ScrollView")]
    [SerializeField] ScrollRect m_srChat;
    [SerializeField] GameObject m_prbText;
    [SerializeField] Transform m_textParent;
    [Header("Send")]
    [SerializeField] InputField m_infSend;
    [SerializeField] Button m_btnSend;
    [Header("MySetting")]
    [SerializeField] InputField m_infMyIP;
    [SerializeField] InputField m_infMyPort;
    [SerializeField] InputField m_infMyName;
    [SerializeField] Button m_btnMySet;
    [Header("OtherSetting")]
    [SerializeField] InputField m_infOtherIP;
    [SerializeField] InputField m_infOtherPort;
    [SerializeField] Button m_btnOtherSet;

    public string myIP;
    public int myPort;
    public string myName;
    public string otherIP;
    public int otherPort;

    void Start()
    {
        m_infSend.onSubmit.AddListener(OnSubmit_Send);
        m_btnSend.onClick.AddListener(OnClicked_Send);
        m_btnMySet.onClick.AddListener(OnClicked_MySet);
        m_btnOtherSet.onClick.AddListener(OnClicked_OtherSet);
    }

    void OnClicked_Send()
    {
        if (myIP != null && m_infSend.text != null)
        {
            string msg = m_infSend.text;
            AddMessage(myName, msg);
            MyChatNet2.Inst.SendMessage(otherIP, otherPort, myName, msg);
            m_infSend.text = string.Empty;
        }
    }
    void OnSubmit_Send(string text)
    {
        text = m_infSend.text;
        AddMessage(myName, text);
        MyChatNet2.Inst.SendMessage(otherIP, otherPort, myName, text);
        m_infSend.text = string.Empty;
    }
    void OnClicked_MySet()
    {
        if (MyInfCheck())
        {
            myIP = m_infMyIP.text;
            myPort = int.Parse(m_infMyPort.text);
            myName = m_infMyName.text;
            MyChatNet2.Inst.StartServer(myIP, myPort);
        }
    }
    bool MyInfCheck()
    {
        if (m_infMyIP != null && m_infMyPort != null && m_infMyName != null)
            return true;
        return false;
    }
    void OnClicked_OtherSet()
    {
        if (OtherInfCheck())
        {
            otherIP = m_infOtherIP.text;
            otherPort = int.Parse(m_infOtherPort.text);
        }
    }
    bool OtherInfCheck()
    {
        if (m_infOtherIP != null && m_infOtherPort != null)
            return true;
        return false;
    }

    public void AddMessage(string name, string msg)
    {
        GameObject go = Instantiate(m_prbText, m_textParent);
        go.GetComponent<Text>().text = name + " : " + msg;
    }
}
