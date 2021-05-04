using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FPSGame.Main;
using Photon.SocketServer;

namespace FPSGame.Handler
{
	public class SyncTransformHandler
	{
		public void OnSyncPositionRecevied(MyGameClient peer, OperationRequest operationRequest, SendParameters sendParameters) {
			Dictionary<byte, object> data = operationRequest.Parameters;
			object o_x, o_y, o_z;
			data.TryGetValue((byte)DataType.X, out o_x);
			data.TryGetValue((byte)DataType.Y, out o_y);
			data.TryGetValue((byte)DataType.Z, out o_z);

			peer.x = float.Parse(o_x.ToString());
			peer.y = float.Parse(o_y.ToString());
			peer.z = float.Parse(o_z.ToString());


		}
	}
}
