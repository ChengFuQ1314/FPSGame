using FPSGame.Main;
using Photon.SocketServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPSGame.Handler
{
	public class SyncRoomHandler
	{
		public void OnSyncJoinRoom(MyGameClient peer, OperationRequest operationRequest, SendParameters sendParameters) {
			int PlayerNum = MyGameServer.instance.peerlist.Count;
			
			Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
			dictionary.Add((byte)DataType.PlayerNum,PlayerNum);
			OperationResponse response = new OperationResponse((byte)OperationCode.SyncJoinRoom);
			response.Parameters = dictionary;
			peer.SendOperationResponse(response, sendParameters);
		}
	}
}
