using FPSGame.Main;
using Photon.SocketServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPSGame.Handler
{
	public class SyncChatHandler
	{
		public void OnSyncChatHandler(MyGameClient peer, OperationRequest operationRequest, SendParameters sendParameters) {
			Dictionary<byte, object> dictionary = operationRequest.Parameters;
			object o_name, o_msg;
			dictionary.TryGetValue((byte)DataType.Chat, out o_msg);
			dictionary.TryGetValue((byte)DataType.PlayerName,out o_name);
			foreach (MyGameClient cp in MyGameServer.instance.peerlist)
			{
				if (!string.IsNullOrEmpty(cp.ClientName) && cp != peer)
				{
					EventData ed = new EventData((byte)EventCode.SyncChat);
					Dictionary<byte, object> newChat = new Dictionary<byte, object>();
					newChat.Add((byte)DataType.Chat, o_msg);
					newChat.Add((byte)DataType.PlayerName, o_name);
					ed.Parameters = newChat;
					cp.SendEvent(ed,sendParameters);
				}
			}
		}
	}
}
