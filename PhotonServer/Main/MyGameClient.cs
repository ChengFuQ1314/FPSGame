using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Photon.SocketServer;
using PhotonHostRuntimeInterfaces;
using FPSGame.Handler;

namespace FPSGame.Main
{
	public class MyGameClient : ClientPeer
	{
		public MyGameClient(InitRequest initRequest) : base(initRequest) { }
		public string ClientName;
		public float x, y, z;
		public float rx, ry, rz;

		protected override void OnDisconnect(DisconnectReason reasonCode, string reasonDetail)
		{
			MyGameServer.instance.Display(ClientName + ":Disconnect!");
			MyGameServer.instance.peerlist.Remove(this);
		}

		protected override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters)
		{
			switch (operationRequest.OperationCode) {
				//新用户加入
				case (byte)OperationCode.SyncPlayer:
					new SyncNewPlayer().OnSyncNewPlayer(this,operationRequest,sendParameters);
					break;
				//位置同步
				case (byte)OperationCode.SyncPosition:
					new SyncTransformHandler().OnSyncPositionRecevied(this,operationRequest,sendParameters);
					break;
				//镜头同步
				case (byte)OperationCode.SyncRotation:
					new SyncRotationHandler().OnSyncRotationRecevied(this, operationRequest, sendParameters);
					break;
				//血量同步
				case (byte)OperationCode.SyncHealth:
					new SyncHealthHandler().OnSyncHealthHandler(this,operationRequest,sendParameters);
					break;
				//死亡同步
				case (byte)OperationCode.SyncDeath:
					new SyncHealthHandler().OnSyncDeathHandler(this,operationRequest,sendParameters);
					break;
				//聊天同步
				case (byte)OperationCode.SyncChat:
					new SyncChatHandler().OnSyncChatHandler(this, operationRequest, sendParameters);
					break;
				//玩家离开
				case (byte)OperationCode.SyncPlayerLeave:
					new SyncPlayerLeaveHandler().OnSyncPlayerLeaveHandler(this, operationRequest, sendParameters);
					break;
				//加入房间
				case (byte)OperationCode.SyncJoinRoom:
					new SyncRoomHandler().OnSyncJoinRoom(this, operationRequest, sendParameters);
					break;
				
					
			}
		}
	}
}
