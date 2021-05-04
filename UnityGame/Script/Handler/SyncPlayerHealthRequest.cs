using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Script.Handler
{
	public class SyncPlayerHealthRequest
	{
		private UserData userdata= new UserData().getUserData();
		public void OnSyncPlayerHealthRequest(int Damage,string username) {
			Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
			dictionary.Add((byte)DataType.Damage,Damage);
			dictionary.Add((byte)DataType.PlayerName,username);
			userdata.myPeer.OpCustom((byte)OperationCode.SyncHealth,dictionary,true);
		}

		public void OnSyncPlayerDeathRequest(string playername,string name) {
			Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
			dictionary.Add((byte)DataType.PlayerName,name);
			dictionary.Add((byte)DataType.Killers,playername);
			userdata.myPeer.OpCustom((byte)OperationCode.SyncDeath,dictionary,true);

		}
	}
}
