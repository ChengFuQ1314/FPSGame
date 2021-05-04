using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Script.Handler
{
	public class SyncPlayerTransformRequest
	{
		public void SendSyncPositionRequest(Vector3 pos, UserData userdata) {
			Dictionary<byte, object> data = new Dictionary<byte, object>();
			data.Add((byte)DataType.X,pos.x);
			data.Add((byte)DataType.Y, pos.y);
			data.Add((byte)DataType.Z, pos.z);
			userdata.myPeer.OpCustom((byte)OperationCode.SyncPosition,data,true);
		}

	}
}
