using ExitGames.Client.Photon;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Script.Handler
{
	public class SyncPlayerHandler
	{
		public void OnSyncPlayerReceived(OperationResponse operationResponse) {
			Dictionary<byte, object> dictionary = operationResponse.Parameters;
			object o_players;
			dictionary.TryGetValue((byte)DataType.PlayerName,out o_players);
			string usernameListString = o_players.ToString();

			using (StringReader reader = new StringReader(usernameListString)) {
				XmlSerializer serializer = new XmlSerializer(typeof(List<string>));
				List<string> usernameList = (List<string>)serializer.Deserialize(reader);
				GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>().OnSyncPlayerResponse(usernameList);
			}
		}

		public void OnSyncPlayerEventReceived(EventData eventData) {
			Dictionary<byte, object> dictionary = eventData.Parameters;
			object o_players;
			dictionary.TryGetValue((byte)DataType.PlayerName,out o_players);
			GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>().NewPlayerJoin(o_players.ToString());
		}

		public void OnSyncTempPlayerHandler(OperationResponse operationResponse) {
			Dictionary<byte, object> dictionary = operationResponse.Parameters;
			object o_playernum;
			dictionary.TryGetValue((byte)DataType.PlayerNum, out o_playernum);
			GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>().CreateTempPlayer(int.Parse(o_playernum.ToString()));
		}
	}
}
