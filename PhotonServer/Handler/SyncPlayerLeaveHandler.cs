using FPSGame.Main;
using Photon.SocketServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPSGame.Handler
{
	public class SyncPlayerLeaveHandler
	{
		public void OnSyncPlayerLeaveHandler(MyGameClient peer, OperationRequest operationRequest, SendParameters sendParameters) {
			Dictionary<byte, object> dictionary = operationRequest.Parameters;
			object o_name;
			dictionary.TryGetValue((byte)DataType.PlayerName, out o_name);

			foreach (MyGameClient cp in MyGameServer.instance.peerlist)
			{
				if (!string.IsNullOrEmpty(cp.ClientName) && cp != peer)
				{
					EventData ed = new EventData((byte)EventCode.SyncPlayerLeave);
					Dictionary<byte, object> playerLeave = new Dictionary<byte, object>();
					playerLeave.Add((byte)DataType.PlayerName, o_name);
					ed.Parameters = playerLeave;
					cp.SendEvent(ed,sendParameters);
				}
			}
		}
	}
}
