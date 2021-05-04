using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoddyRoomUI : MonoBehaviour
{
    private UserData userdata=new UserData().getUserData();
    public Text playerAttribute;
    public Text playerLevel;
    public Text playerKills;
    public Text playerExper;
    public Text playerDeath;


    //切换场景
    public void FindGame() {
        userdata.GameFlag = "Game";
        SceneManager.LoadScene("Game");
    }
    public void JoinRoom() {
        
    }
    public void ReturnLogin() {
        userdata.SocketDisconnect();
        userdata.GameDataSend();
        SceneManager.LoadScene("Login");
    }
    public void ExitGame() {
        userdata.SocketDisconnect();
#if UNITY_EDITOR    //在编辑器模式下
        EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    void Start()
    {
        playerAttribute.text = "名字:"+userdata.userattribute.name;
        playerLevel.text = "等级:" + userdata.userattribute.Level;
        playerExper.text = "经验值:" + userdata.userattribute.Experience;
        playerKills.text = "杀敌数:" + userdata.userattribute.Kills;
        playerDeath.text = "死亡数:" + userdata.userattribute.Deaths;
        userdata.GameFlag = "Loddy";
    }

    
    void Update()
    {
        
    }
}
