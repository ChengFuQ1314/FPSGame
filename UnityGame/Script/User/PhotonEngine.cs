using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;
using Assets.Script.Handler;

public class PhotonEngine : MonoBehaviour,IPhotonPeerListener
{
	private static PhotonEngine instance;
	private UserData userdata=new UserData().getUserData();
	public Canvas canvas;

	public void DebugReturn(DebugLevel level, string message)
	{
		
	}

	//服务器主动发送事件的处理函数
	public void OnEvent(EventData eventData)
	{
		switch (eventData.Code) {
			case (byte)EventCode.NewPlayer:
				new SyncPlayerHandler().OnSyncPlayerEventReceived(eventData);
				break;
			case (byte)EventCode.SyncPosition:
				new SyncPlayerTransformEvent().OnSyncPositionRecevied(eventData);
				break;
			case (byte)EventCode.SyncDeath:
				new SyncPlayerHealthHandler().OnSyncPlayerDeathHandler(eventData);
				break;
			case (byte)EventCode.SyncChat:
				new SyncPlayerChatHandler().OnSyncChatEventHandler(eventData);
				break;
			case (byte)EventCode.SyncPlayerLeave:
				new SyncPlayerLeaveHandler().OnSyncPlayerLeaveEventHandler(eventData);
				break;
		}
	}

	//接收服务器响应
	public void OnOperationResponse(OperationResponse operationResponse)
	{
		switch (operationResponse.OperationCode) {
			case (byte)OperationCode.SyncPlayer:
				new SyncPlayerHandler().OnSyncPlayerReceived(operationResponse);
				break;
			case (byte)OperationCode.SyncHealth:
				new SyncPlayerHealthHandler().OnSyncPlayerHealthHandler(operationResponse);
				break;
			case (byte)OperationCode.SyncJoinRoom:
				new SyncPlayerHandler().OnSyncTempPlayerHandler(operationResponse);
				break;
			
		}
	}

	//如果连接状态发生改变的时候触犯此方法
	public void OnStatusChanged(StatusCode statusCode)
	{
		switch (statusCode) {
			case StatusCode.Connect:
				Debug.Log("Connect");
				canvas.GetComponent<LoddyMsg>().LoddyMsgStatus("Connect");
				userdata.StatusChange("Connect");
				break;
			case StatusCode.Disconnect:
				Debug.Log("Disconnect");
				canvas.GetComponent<LoddyMsg>().LoddyMsgStatus("Disconnect");
				userdata.StatusChange("Disconnect");
				break;
			case StatusCode.Exception:
				Debug.Log("Exception");
				canvas.GetComponent<LoddyMsg>().LoddyMsgStatus("Exception");
				//userdata.SocketDisconnect();
				break;
		}
	}

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
	}

	void Start()
    {
		userdata.myPeer = new PhotonPeer(this,ConnectionProtocol.Udp);
		userdata.myPeer.Connect(userdata.userattribute.Address,"FPSGame");
    }

    
    void Update()
    {
		userdata.PhotonServiceStart();
    }
}
