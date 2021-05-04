using Assets.Script.Handler;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    public GameObject Homeowner;
    public GameObject OtherPlayer;
    public GameObject Teammate;
    public GameObject Grenade;
    public int GameClientNum=0;
    public Dictionary<string, GameObject> playerDic = new Dictionary<string, GameObject>();

    private UserData userdata= new UserData().getUserData();
    public Vector3[] Resurrections = new Vector3[5];

    void Start()
    {
        //发送新角色加入
        new SyncPlayerRequest().SendSyncPlayerRequest(userdata);
        new SyncPlayerRequest().SendPlayerJoinRoom(userdata);
        ResurrectionInit();
    }
    
    void Update()
    {
        userdata.PhotonServiceStart();
    }

    private void ResurrectionInit() {
        Resurrections[0] = new Vector3(0, 0.5f, -10);
        Resurrections[1] = new Vector3(0, 0.5f, 10);
        Resurrections[2] = new Vector3(-51, 0.5f, -23);
        Resurrections[3] = new Vector3(-70, 0.5f, 12);
        Resurrections[4] = new Vector3(-44, 0.5f, -15);
    }

    public void CreateTempPlayer(int playernum) {

        Instantiate(Homeowner, Resurrections[(int)Random.Range(0,5)], Quaternion.identity);

    }

    //实例化客户端
    public void NewPlayerJoin(string username) {
        GameObject go = Instantiate(OtherPlayer);
        go.name = username;
        go.GetComponent<OtherPlayerData>().ClientName = username;
        playerDic.Add(username, go);
        GameObject.FindGameObjectWithTag("GameChat").GetComponent<GameChat>().OnDisplay(username, "加入游戏");
    }

    //新用户创建与加入
    public void OnSyncPlayerResponse(List<string> usernameList)
    {
        foreach (string username in usernameList)
        {
            NewPlayerJoin(username);
            GameClientNum++;
        }
        
    }

    public void OnSyncPlayerLeaveHandler(string username) {
        GameObject go;
        playerDic.TryGetValue(username, out go);
        if (go != null) {
            Destroy(go);
            playerDic.Remove(username);
            GameObject.FindGameObjectWithTag("GameChat").GetComponent<GameChat>().OnDisplay(username, "退出游戏");
        }
    }


}
