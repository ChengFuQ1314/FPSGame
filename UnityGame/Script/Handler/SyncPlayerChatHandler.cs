using ExitGames.Client.Photon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Script.Handler
{
	public class SyncPlayerChatHandler
	{
		public void OnSyncPlayerChatMessageSend(UserData userdata,string msg) {
			Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
			dictionary.Add((byte)DataType.PlayerName, userdata.userattribute.name);
			dictionary.Add((byte)DataType.Chat, msg);
			userdata.myPeer.OpCustom((byte)OperationCode.SyncChat,dictionary,true);
		}
		public void OnSyncChatEventHandler(EventData eventData) {
			Dictionary<byte, object> dictionary = eventData.Parameters;
			object o_name, o_msg;
			dictionary.TryGetValue((byte)DataType.Chat, out o_msg);
			dictionary.TryGetValue((byte)DataType.PlayerName, out o_name);
			GameObject.FindGameObjectWithTag("GameChat").GetComponent<GameChat>().OnDisplay(o_name.ToString(), o_msg.ToString());
		}
	}
}
