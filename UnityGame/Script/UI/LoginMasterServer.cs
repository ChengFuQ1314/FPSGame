using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginMasterServer : MonoBehaviour
{
    public UserData userdata;

    public string IP = "";
    public int port = 0;

    public InputField loginUser;
    public InputField loginPassword;
    public InputField registerUser;
    public InputField registerPassword;
    public InputField registerName;
    public Text loginTips;
    public Text registerTips;
    public Image loginUI;
    public Image registerUI;

    private void ServerConnect()
    {
        IPAddress ip = IPAddress.Parse(IP);
        userdata.socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IPEndPoint endPoint = new IPEndPoint(ip, port);
        userdata.socket.Connect(endPoint);
    }

    private void Awake()
    {
        userdata = new UserData().getUserData();
        //连接主服务器
        ServerConnect();
    }

    //用户登录数据处理
    public void LoginHandle() {
        string userID = loginUser.text;
        string passWord = loginPassword.text;
        if (userID != "" && passWord != "") {
            UserLoginMsgSend(userID,passWord);
        }
    }

    public void RegisterHandle() {
        loginUI.gameObject.SetActive(false);
        registerUI.gameObject.SetActive(true);
    }
    public void ReturnHandler() {
        loginUI.gameObject.SetActive(true);
        registerUI.gameObject.SetActive(false);
    }
    public void RegisterMessageHandle() {
        string userID = registerUser.text;
        string passWord = registerPassword.text;
        string userName = registerName.text;
        if (userID != "" && passWord != "" && userName != "")
        {
            UserRegisterMsgSend(userID,passWord,userName);
        }
    }

    private void UserRegisterMsgSend(string userID, string password,string name) {
        //要发送的数据处理（长度+数据）
        string userMsg = "Register@" + userID + ":" + password + "#" +name;
        byte[] msg = Encoding.ASCII.GetBytes(userMsg);
        short size = (short)userMsg.Length;
        byte[] len = BitConverter.GetBytes(size);
        Array.Reverse(len);
        byte[] sendMsg = new byte[msg.Length + len.Length];
        len.CopyTo(sendMsg, 0);
        msg.CopyTo(sendMsg, len.Length);
        userdata.socket.Send(sendMsg, sendMsg.Length, 0);

        //数据接收的处理
        byte[] recv = new byte[1024];
        int recvlen = userdata.socket.Receive(recv);
        if (recvlen > 0)
        {
            string s_recv = System.Text.Encoding.UTF8.GetString(recv);
            MsgHandle(s_recv);
        }
    }

    private void UserLoginMsgSend(string userID,string password) {

        //要发送的数据处理（长度+数据）
        string userMsg = "login@" + userID + ":" + password + "#";
        byte[] msg = Encoding.ASCII.GetBytes(userMsg);
        short size = (short)userMsg.Length;
        byte[] len = BitConverter.GetBytes(size);
        Array.Reverse(len);
        byte[] sendMsg = new byte[msg.Length+len.Length];
        len.CopyTo(sendMsg,0);
        msg.CopyTo(sendMsg,len.Length);
        userdata.socket.Send(sendMsg, sendMsg.Length, 0);

        //数据接收的处理
        byte[] recv = new byte[1024];
        int recvlen = userdata.socket.Receive(recv);
        if (recvlen > 0) {
            string s_recv = System.Text.Encoding.UTF8.GetString(recv);
            MsgHandle(s_recv);
        }
    }

    //把接收到的数据进行处理
    private void MsgHandle(string data) {
        int s_index = data.IndexOf("@");
        int e_index = data.IndexOf("#");
        int a_index = data.IndexOf("&");
        int d_index = data.IndexOf("$");
        if (s_index > 0)
        {
            string cmd = data.Substring(0, s_index);
            if (cmd == "Success")
            {
                loginTips.text = "登录成功";
                string s_name = data.Substring(s_index + 1, a_index - s_index - 1);
                string ServerAddress = data.Substring(a_index + 1, d_index - a_index - 1);
                string s_gamedata = data.Substring(d_index + 1);
                int[] gamedata = UserGameDataHandler(s_gamedata);
                userdata.userattribute.SetUserAttribute(s_name, ServerAddress, gamedata[0], gamedata[1], gamedata[2], gamedata[3]);
                SceneManager.LoadScene("Loddy");
            }
            else if (cmd == "RegisterSuccess") {
                registerTips.text = "注册成功,请返回登录界面";
            }
        }
        else if (e_index > 0) {
            string cmd = data.Substring(0, e_index);
            if (cmd == "This account is online")
            {
                loginTips.text = "此账号已被登录";
            }
            else if (cmd == "User or password error")
            {
                loginTips.text = "账号或密码错误";
            }
            else if (cmd == "This account already exists") {
                registerTips.text = "此账号已被注册";
            }
        }
    }

    private int[] UserGameDataHandler(string data) {
        int[] gamedata = new int[4];
        for (int i=0;i<4;i++) {
            int i_value = data.IndexOf("/");
            gamedata[i] = int.Parse(data.Substring(0, i_value));
            data=data.Remove(0,i_value+1);
        }
        return gamedata;
    }

}
