using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoddyMsg : MonoBehaviour
{
    public Text loddymsg;

    public void LoddyMsgStatus(string code) {
        switch (code) {
            case "Connect":
                loddymsg.text = "连接服务器成功";
                break;
            case "Disconnect":
                loddymsg.text = "服务器已断开";
                break;
            case "Exception":
                loddymsg.text = "服务器异常";
                break;
        }
    }

}
