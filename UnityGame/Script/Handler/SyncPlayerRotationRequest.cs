using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Script.Handler
{
	public class SyncPlayerRotationRequest
	{
		public void SendSyncRotationRequest(Vector3 Ros, UserData userdata) {
			Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
			dictionary.Add((byte)DataType.RX,Ros.x);
			dictionary.Add((byte)DataType.RY,Ros.y);
			dictionary.Add((byte)DataType.RZ,Ros.z);
			userdata.myPeer.OpCustom((byte)OperationCode.SyncRotation,dictionary,true);
		}
	}
}
