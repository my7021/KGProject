using UnityEngine;
using UnityEngine.UI;

public class MySetting
{
    public string m_ip;
    public int m_port;
    public string m_name;
    public MySetting(string ip, int port, string name)
    {
        m_ip = ip;
        m_port = port;
        m_name = name;
    }
}
public class OtherSetting
{
    public string m_ip;
    public int m_port;
    public OtherSetting(string ip, int port)
    {
        m_ip = ip;
        m_port = port;
    }
}

public class ChatDlg : MonoBehaviour
{
    [SerializeField] GameObject m_prbtext;
    [SerializeField] Transform m_textParent;
    [Header("InputField")]
    [SerializeField] InputField m_infSend;
    [SerializeField] InputField m_infMyIP;
    [SerializeField] InputField m_infMyPort;
    [SerializeField] InputField m_infMyName;
    [SerializeField] InputField m_infOtherIP;
    [SerializeField] InputField m_infOtherPort;
    [Header("Button")]
    [SerializeField] Button m_btnSend;
    [SerializeField] Button m_btnMySet;
    [SerializeField] Button m_btnOtherSet;

    public MySetting m_MySetting = null;
    public OtherSetting m_OtherSetting = null;

    int mySetCount = 0;
    int otherSetCount = 0;

    void Start()
    {
        m_btnSend.onClick.AddListener(OnClicked_Send);
        m_btnMySet.onClick.AddListener(OnClicked_MySet);
        m_btnOtherSet.onClick.AddListener(OnClicked_OtherSet);
    }

    public void CreateMsg(string name, string msg)
    {
        GameObject go = Instantiate(m_prbtext, m_textParent);
        go.GetComponent<Text>().text = name + ":" + msg;
    }

    void OnClicked_Send()
    {
        if (m_infSend.text != null)
        {
            string msg = m_infSend.text;
            MyChatNet.Inst.m_ChatSender.SendMsgAsync(m_OtherSetting.m_ip, m_OtherSetting.m_port, m_MySetting.m_name, msg);
            CreateMsg(m_MySetting.m_name, msg);
            m_infSend.text = string.Empty;
        }
    }
    void OnClicked_MySet()
    {
        if (mySetCount == 0 && MyInfCheck())
        {
            string ip = m_infMyIP.text;
            int port = int.Parse(m_infMyPort.text);
            string name = m_infMyName.text;

            m_MySetting = new MySetting(ip, port, name);
            MyChatNet.Inst.StartServer(m_MySetting.m_ip, m_MySetting.m_port);
            mySetCount++;
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
            string ip = m_infOtherIP.text;
            int port = int.Parse(m_infOtherPort.text);

            m_OtherSetting = new OtherSetting(ip, port);
        }
    }
    bool OtherInfCheck()
    {
        if (m_infOtherIP != null && m_infOtherPort != null)
            return true;
        return false;
    }

    void Update()
    {
        
    }
}
