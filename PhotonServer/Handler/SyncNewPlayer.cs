using FPSGame.Main;
using Photon.SocketServer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FPSGame.Handler
{
	public class SyncNewPlayer
	{
		public void OnSyncNewPlayer(MyGameClient peer,OperationRequest operationRequest, SendParameters sendParameters) {
			
			Dictionary<byte, object> dictionary = operationRequest.Parameters;
			object o_name;
			dictionary.TryGetValue((byte)DataType.PlayerName,out o_name);

			//先进行一个防止重复的操作
			foreach (MyGameClient cp in MyGameServer.instance.peerlist)
			{
				if (peer.ClientName == o_name.ToString())
				{
					return;
				}
			}
			peer.ClientName = o_name.ToString();

			//取得所有在线玩家的用户名
			List<string> usernameList = new List<string>();
			foreach (MyGameClient cp in MyGameServer.instance.peerlist) {
				//如果连接过来的客户端已经登录了并且这个客户端不是当前客户端
				if (!string.IsNullOrEmpty(cp.ClientName) && cp != peer) {
					//把客户端的用户添加到集合里面
					usernameList.Add(cp.ClientName);
				}
			}

			//通过xml序列化进行数据传输，传输给客户端
			StringWriter sw = new StringWriter();
			XmlSerializer serializer = new XmlSerializer(typeof(List<string>));
			serializer.Serialize(sw,usernameList);
			sw.Close();
			string usernameListString = sw.ToString();

			//告诉当前客户端其他客户端的名字
			Dictionary<byte, object> data = new Dictionary<byte, object>();
			data.Add((byte)DataType.PlayerName,usernameListString);
			OperationResponse response = new OperationResponse((byte)OperationCode.SyncPlayer);
			response.Parameters = data;
			peer.SendOperationResponse(response,sendParameters);

			//告诉其他客户端有新的客户端加入
			foreach (MyGameClient cp in MyGameServer.instance.peerlist) {
				if (!string.IsNullOrEmpty(cp.ClientName) && cp != peer) {
					EventData ed = new EventData((byte)EventCode.NewPlayer);
					Dictionary<byte, object> newplayerData = new Dictionary<byte, object>();
					newplayerData.Add((byte)DataType.PlayerName,peer.ClientName); //把新加入的用户名传递给其他客户端
					ed.Parameters = newplayerData;
					cp.SendEvent(ed,sendParameters); //发送事件
				}
			}
		}
	}
}
