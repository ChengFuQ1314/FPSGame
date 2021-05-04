using ExitGames.Client.Photon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Script.Handler
{
	//发送玩家信息
	public class SyncPlayerRequest
	{
		public void SendSyncPlayerRequest(UserData userdata) {
			Dictionary<byte, object> dictionary = new Dictionary<byte, object>
			{
				{ (byte)DataType.PlayerName, userdata.userattribute.name }
			};
			userdata.myPeer.OpCustom((byte)OperationCode.SyncPlayer, dictionary, true);
		}

		public void SendPlayerJoinRoom(UserData userdata) {
			userdata.myPeer.OpCustom((byte)OperationCode.SyncJoinRoom,new Dictionary<byte, object>(),true);
		}
	}
}
