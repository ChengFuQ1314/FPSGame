using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using FPSGame.Main;
using Photon.SocketServer;

namespace FPSGame.Handler
{
	public class SyncPositionThread
	{
		private Thread thread;

		public void Run() {
			thread = new Thread(UpdataPosition);
			thread.IsBackground = true; //后台运行
			thread.Start(); //启动线程
		}

		private void UpdataPosition() {
			Thread.Sleep(3000);
			while (true) {
				Thread.Sleep(20);
				SendPosition();
			}
		}

		private void SendPosition() {
			List<PlayerData> playerDataList = new List<PlayerData>();
			foreach (MyGameClient peer in MyGameServer.instance.peerlist) {
				//获取当前登录的客户端
				if (string.IsNullOrEmpty(peer.ClientName) == false) {
					PlayerData playerData = new PlayerData();
					playerData.Username = peer.ClientName;
					playerData.x = peer.x;
					playerData.y = peer.y;
					playerData.z = peer.z;
					playerData.rx = peer.rx;
					playerData.ry = peer.ry;
					playerData.rz = peer.rz;
					playerDataList.Add(playerData);
				}
			}

			//进行xml序列化成string
			StringWriter sw = new StringWriter();
			XmlSerializer serializer = new XmlSerializer(typeof(List<PlayerData>));
			serializer.Serialize(sw,playerDataList);
			sw.Close();
			string playerDataListString = sw.ToString();

			Dictionary<byte, object> data = new Dictionary<byte, object>();
			data.Add((byte)DataType.Position,playerDataListString);
			foreach (MyGameClient peer in MyGameServer.instance.peerlist) {
				if (string.IsNullOrEmpty(peer.ClientName) == false) {
					EventData ed = new EventData((byte)EventCode.SyncPosition);
					ed.Parameters = data;
					peer.SendEvent(ed,new SendParameters());
				}
			}

		}

		public void Stop() {
			thread.Abort(); //终止线程
		}

		
		
	}
}
