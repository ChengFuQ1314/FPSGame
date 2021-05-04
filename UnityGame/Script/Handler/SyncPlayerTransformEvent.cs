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
	public class SyncPlayerTransformEvent
	{
		public void OnSyncPositionRecevied(EventData eventData) {
			Dictionary<byte, object> dictionary = eventData.Parameters;
			object o_playerTransform;
			dictionary.TryGetValue((byte)DataType.Position,out o_playerTransform);

			using (StringReader reader = new StringReader(o_playerTransform.ToString())) {
				XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<PlayerData>));
				List<PlayerData> playerDataList = (List<PlayerData>)xmlSerializer.Deserialize(reader);
				GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerSync>().OnSyncPositionEvent(playerDataList);
			}
		}
	}
}
