using FPSGame.Main;
using Photon.SocketServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPSGame.Handler
{
	public class SyncHealthHandler
	{
		public void OnSyncHealthHandler(MyGameClient peer, OperationRequest operationRequest, SendParameters sendParameters) {
			Dictionary<byte, object> dictionary = operationRequest.Parameters;
			object o_username, o_damage;
			dictionary.TryGetValue((byte)DataType.Damage,out o_damage);
			dictionary.TryGetValue((byte)DataType.PlayerName,out o_username);

			//在当前在线用户列表找到受伤害的玩家 并发送伤害信息
			foreach (MyGameClient cp in MyGameServer.instance.peerlist)
			{
				if (cp.ClientName == o_username.ToString())
				{
					MyGameServer.instance.Display(o_damage.ToString());
					Dictionary<byte, object> peerdictionary = new Dictionary<byte, object>();
					peerdictionary.Add((byte)DataType.Damage, o_damage.ToString());
					peerdictionary.Add((byte)DataType.PlayerName,peer.ClientName.ToString());
					OperationResponse response = new OperationResponse((byte)OperationCode.SyncHealth);
					response.Parameters = peerdictionary;
					cp.SendOperationResponse(response, new SendParameters());
					break;
				}
			}
		}
		public void OnSyncDeathHandler(MyGameClient peer, OperationRequest operationRequest, SendParameters sendParameters) {
			Dictionary<byte, object> dictionary = operationRequest.Parameters;
			object o_username,o_killer;
			dictionary.TryGetValue((byte)DataType.PlayerName,out o_username);
			dictionary.TryGetValue((byte)DataType.Killers,out o_killer);

			foreach (MyGameClient cp in MyGameServer.instance.peerlist) {
				if (!string.IsNullOrEmpty(cp.ClientName)) {
					EventData ed = new EventData((byte)EventCode.SyncDeath);
					Dictionary<byte, object> eventdictionary = new Dictionary<byte, object>();
					eventdictionary.Add((byte)DataType.PlayerName,o_username.ToString());
					eventdictionary.Add((byte)DataType.Killers,o_killer.ToString());
					ed.Parameters = eventdictionary;
					cp.SendEvent(ed,sendParameters);
				}
			}
		}
	}
}
