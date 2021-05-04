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
	public class SyncPlayerHealthHandler
	{
		public void OnSyncPlayerHealthHandler(OperationResponse operationResponse) {
			Dictionary<byte, object> dictionary = operationResponse.Parameters;
			object o_damage,o_player;
			dictionary.TryGetValue((byte)DataType.Damage,out o_damage);
			dictionary.TryGetValue((byte)DataType.PlayerName,out o_player);
			GameObject.FindGameObjectWithTag("Player").GetComponent<Health>().OnHealth(int.Parse(o_damage.ToString()),o_player.ToString());
		}
		public void OnSyncPlayerDeathHandler(EventData eventData) {
			Dictionary<byte, object> dictionary = eventData.Parameters;
			object o_killer, o_playername;
			dictionary.TryGetValue((byte)DataType.PlayerName, out o_playername) ;
			dictionary.TryGetValue((byte)DataType.Killers,out o_killer);
			GameObject.FindGameObjectWithTag("Player").GetComponent<Health>().KillList(o_killer.ToString(),o_playername.ToString());
		}
	}
}
