using System;
using System.Net.Sockets;
using System.Text;
using ExitGames.Client.Photon;

public class UserData
{
    private static UserData user;
    private string ConnectFlag;
    public string GameFlag;
    public PhotonPeer myPeer;
    public UserAttribute userattribute;
    public Socket socket;

	public UserData getUserData() {
        if (user == null) {
            user = this;
            userattribute = new UserAttribute();
        }
        return user;
    }
    public void SocketDisconnect() {
        GameDataSend();
        user = null;
        userattribute = null;
        socket.Shutdown(SocketShutdown.Both);
        myPeer.Disconnect();
        socket.Close();
    }

    public void PhotonServiceStart() {
        myPeer.Service();
    }

    public void StatusChange(string status) {
        switch (status) {
            case "Connect":
                ConnectFlag = status;
                break;
            case "Disconnect":
                ConnectFlag = status;
                break;
        }
    }

    public void GameDataSend() {
        string userMsg = "GameData@" + userattribute.Level + "/" + userattribute.Experience + "/" + userattribute.Kills + "/" + userattribute.Deaths + "/";
        byte[] msg = Encoding.ASCII.GetBytes(userMsg);
        short size = (short)userMsg.Length;
        byte[] len = BitConverter.GetBytes(size);
        Array.Reverse(len);
        byte[] sendMsg = new byte[msg.Length + len.Length];
        len.CopyTo(sendMsg, 0);
        msg.CopyTo(sendMsg, len.Length);
        socket.Send(sendMsg, sendMsg.Length, 0);
    }
}

public class UserAttribute {
    public string name;
    public string Address;
    public int Level;
    public int Experience;
    public int Kills;
    public int Deaths;

    public void SetUserAttribute(string name,string Address,int Level,int Experience,int Kills,int Deaths) {
        this.name = name;
        this.Address = Address;
        this.Level = Level;
        this.Experience = Experience;
        this.Kills = Kills;
        this.Deaths = Deaths;
    }
}
