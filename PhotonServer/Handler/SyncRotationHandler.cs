using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FPSGame.Main;
using Photon.SocketServer;

namespace FPSGame.Handler
{
	public class SyncRotationHandler
	{
		public void OnSyncRotationRecevied(MyGameClient peer, OperationRequest operationRequest, SendParameters sendParameters) {
			Dictionary<byte, object> dictionary = operationRequest.Parameters;
			object o_rx, o_ry, o_rz;
			dictionary.TryGetValue((byte)DataType.RX,out o_rx);
			dictionary.TryGetValue((byte)DataType.RY, out o_ry);
			dictionary.TryGetValue((byte)DataType.RZ, out o_rz);

			peer.rx = float.Parse(o_rx.ToString());
			peer.ry = float.Parse(o_ry.ToString());
			peer.rz = float.Parse(o_rz.ToString());
		}
	}
}
