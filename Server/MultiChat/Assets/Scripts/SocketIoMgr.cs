using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SocketIOClient;
using SocketIOClient.Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

public class SocketIoMgr : MonoBehaviour
{
    static SocketIoMgr _inst = null;
    public static SocketIoMgr Inst
    {
        get
        {
            if (_inst == null)
            {
                GameObject go = new GameObject("Socket io manager");
                _inst = go.AddComponent<SocketIoMgr>();
                DontDestroyOnLoad(go);
            }
            return _inst;
        }
    }

    SocketIOUnity m_Socket = null;

    public string ipStr = "192.168.0.76";
    public int dataPort = 15001;
    public int backPort = 16001;

    public UserInfo m_MyUserInfo = new UserInfo();
    public UserInfoList m_UserInfoList = new UserInfoList();

    public event EventHandler<int> onAck_Join = null;
    public event EventHandler<int> onAck_Login = null;
    public event EventHandler<string> onAck_Logout = null;
    public event EventHandler<UserInfo> onNotify_UserInfo = null;
    public event EventHandler<UserInfoList> onNotify_UserInfoList = null;

    public async void InitSocketIO()
    {
        var uri = new Uri("http://192.168.0.72:19000");
        var socket = new SocketIOUnity(uri, new SocketIOOptions
        {
            Query = new Dictionary<string, string>
            {
                {"token", "UNITY"}
            },
            EIO = 4,
            Transport = SocketIOClient.Transport.TransportProtocol.WebSocket
        });

        socket.JsonSerializer = new NewtonsoftJsonSerializer();
        m_Socket = socket;

        await socket.ConnectAsync();


        socket.OnUnityThread("join", (res) =>
        {
            Debug.Log("[SocketIO] join -> " + res);
            UserJoin packet = res.GetValue<UserJoin>();
            if (packet.success == 0)
            {
                Debug.Log($"[SocketIO] 채널에 가입 성공!!, id : {packet.id}");
            }
            else
            {
                Debug.Log($"[SocketIO] 채널에 가입 실패!!, id : {packet.id}");
            }
            if (onAck_Join != null)
                onAck_Join(this, packet.success);
        });


        socket.OnUnityThread("login", (res) =>
        {
            Debug.Log("[SocketIO] login => " + res + $"   {Time.time} ");

            var jsonArr = JArray.Parse(res.ToString());
            var json = jsonArr[0];

            var sID = (string)json["id"];
            var nSuccess = (int)json["success"];

            UserLogin kLogin = res.GetValue<UserLogin>();

            if (nSuccess == 0)
            {
                Debug.LogFormat("로그인 성공!!, ( id={0}", sID);
                UserInfo kInfo = new UserInfo();

                kInfo.id = sID;
                kInfo.ip = ipStr;
                kInfo.dataPort = dataPort;
                kInfo.backPort = backPort;

                m_MyUserInfo = kInfo;

                string data = JsonConvert.SerializeObject(kInfo);
                socket.Emit("user_info", data);
            }
            else
                Debug.Log("로그인 실패!!");

            if (onAck_Login != null)
                onAck_Login(this, kLogin.success);
        });

        socket.OnUnityThread("user_info", (res) =>
        {
            Debug.Log("[SocketIO] user_info" + res);

            UserInfo kInfo = res.GetValue<UserInfo>();

            UserInfo findUser = m_UserInfoList.datas.Find((item) =>
            {
                return (kInfo.id == item.id) ? true : false;
            });

            if (findUser == null)
            {
                if (this.m_MyUserInfo.id != kInfo.id)
                {
                    m_UserInfoList.datas.Add(kInfo);

                    if (onNotify_UserInfo != null)
                        onNotify_UserInfo(this, kInfo);
                }
            }

        });

        socket.OnUnityThread("user_info_list", (res) =>
        {
            Debug.Log(res);
            m_UserInfoList.datas.Clear();
            m_UserInfoList = res.GetValue<UserInfoList>();
            if(m_UserInfoList != null)
            {
                Debug.Log("[SocketIO} 1 = \n" + m_UserInfoList.ToString());
                var kUserInfo = m_UserInfoList.datas.Find((info) => (info.id == m_MyUserInfo.id));
                if(kUserInfo != null)
                    m_MyUserInfo = kUserInfo;
            }

            if(onNotify_UserInfoList != null)
                onNotify_UserInfoList(this, m_UserInfoList);

        });

        socket.OnUnityThread("logout", (res) =>
        {
            Debug.Log(res);

            UserInfo kInfo = res.GetValue<UserInfo>();
            UserInfo userInfo = m_UserInfoList.datas.Find((item) => item.id == kInfo.id);
            if(userInfo != null)
            {
                m_UserInfoList.datas.Remove(userInfo);
            }

            if (onAck_Logout != null)
                onAck_Logout(this, m_MyUserInfo.id);
        });
    }


    public async void SendJoin(string id, string pass)
    {
        await m_Socket.EmitAsync("join", id, pass);
    }

    public async void SendLogin(string id, string pass)
    {
        await m_Socket.EmitAsync("login", id, pass);
    }

    public async void SendLogout(string id)
    {
        await m_Socket.EmitAsync("logout", id);
    }

    public async void SendWithdraw(string id)
    {
        await m_Socket.EmitAsync("withdraw", id);
    }



    [Serializable]
    public class UserJoin
    {
        public string id { get; set; } = string.Empty;
        public int success { get; set; } = 0;
        public string socketId { get; set; } = string.Empty;
    }

    public class UserLogin
    {
        public string id { get; set; } = string.Empty;
        public int success { get; set; } = 0;
    }

    public class UserInfo
    {
        public string id { get; set; } = string.Empty;
        public string socketId { get; set; } = string.Empty;
        public string ip { get; set; } = string.Empty;
        public int dataPort { get; set; } = 0;
        public int backPort { get; set; } = 0;
    }

    public class UserInfoList
    {
        public List<UserInfo> datas { get; set; } = new List<UserInfo>();
    }
}
