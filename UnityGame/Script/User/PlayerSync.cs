using Assets.Script.Handler;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSync : MonoBehaviour
{
    //游戏管理
    private GameManager Manager;
    //角色信息
    private UserData userdata = new UserData().getUserData();
    //记录上一次位置
    private Vector3 lastRotation;
    private Vector3 lastPosition;
    public Text Name;


    // Start is called before the first frame update
    void Start()
    {
        //获取游戏管理脚本
        Manager = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>();
        //初始化上一个的位置
        lastRotation = transform.localEulerAngles;
        lastPosition = new Vector3(0, 1, 0);
        //开启位置同步的信息
        InvokeRepeating("SyncPosition", 1, 1 / 50f);
        Name.text = userdata.userattribute.name;
    }

    // Update is called once per frame
    void Update()
    {
        userdata.PhotonServiceStart();
    }

    //发送当前客户端位置
    public void SyncPosition()
    {
        if (Vector3.Distance(transform.position, lastPosition) > 0.1f)
        {
            lastPosition = transform.position;
            new SyncPlayerTransformRequest().SendSyncPositionRequest(transform.position, userdata);
        }
        if (Vector3.Distance(transform.localEulerAngles, lastRotation) > 0.1f)
        {
            lastRotation = transform.localEulerAngles;
            new SyncPlayerRotationRequest().SendSyncRotationRequest(transform.localEulerAngles, userdata);
        }
    }
    //同步其他客户端位置
    public void OnSyncPositionEvent(List<PlayerData> playerDataList)
    {
        foreach (PlayerData pd in playerDataList)
        {
            Dictionary<string, GameObject> dictionary = Manager.playerDic;
            GameObject go = new GameObject();
            dictionary.TryGetValue(pd.Username, out go);
            if (go != null)
            {
                go.transform.position = new Vector3() { x = pd.x, y = pd.y, z = pd.z };
                go.transform.localEulerAngles = new Vector3() { x = pd.rx, y = pd.ry, z = pd.rz };
            }
        }
    }
    
}
