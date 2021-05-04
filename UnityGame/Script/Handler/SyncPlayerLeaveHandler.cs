using ExitGames.Client.Photon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Script.Handler
{
	public class SyncPlayerLeaveHandler
	{
		public void OnSyncPlayerLeaveSend(UserData userdata) {
			Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
			dictionary.Add((byte)DataType.PlayerName, userdata.userattribute.name);
			userdata.myPeer.OpCustom((byte)OperationCode.SyncPlayerLeave, dictionary, true);
		}

		public void OnSyncPlayerLeaveEventHandler(EventData eventData) {
			Dictionary<byte, object> dictionary = eventData.Parameters;
			object o_name;
			dictionary.TryGetValue((byte)DataType.PlayerName,out o_name);
			GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>().OnSyncPlayerLeaveHandler(o_name.ToString());
		}
	}
}
