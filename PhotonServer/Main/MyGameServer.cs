using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExitGames.Logging;
using ExitGames.Logging.Log4Net;
using log4net.Config;
using Photon.SocketServer;
using FPSGame.Handler;

namespace FPSGame.Main
{
	public class MyGameServer : ApplicationBase
	{

		public static readonly ILogger log = LogManager.GetCurrentClassLogger();
		public static MyGameServer instance;
		public List<MyGameClient> peerlist = new List<MyGameClient>();
		private SyncPositionThread syncPositionThread = new SyncPositionThread();

		protected override PeerBase CreatePeer(InitRequest initRequest)
		{
			log.Info("A New Client Connect!");
			MyGameClient peer = new MyGameClient(initRequest);
			peerlist.Add(peer);
			return peer;
		}

		protected override void Setup()
		{
			//日志的初始化
			LogInit();
			InstanceInit();
			//开启发送同步信息的线程
			syncPositionThread.Run();
		}


		protected override void TearDown()
		{
			syncPositionThread.Stop();
			Display("服务器已关闭");
		}

		private void InstanceInit() {
			if (instance == null)
				instance = this;
		}

		public void Display(string msg) {
			log.Info(msg);
		}

		private void LogInit() {
			log4net.GlobalContext.Properties["Photon:ApplicationLogPath"] = Path.Combine(Path.Combine(Path.Combine(this.ApplicationRootPath, "bin_win64")), "log");
			FileInfo configFileInfo = new FileInfo(Path.Combine(this.BinaryPath, "log4net.config"));//告诉log4net日志的配置文件的位置
			//如果这个配置文件存在
			if (configFileInfo.Exists)
			{
				LogManager.SetLoggerFactory(Log4NetLoggerFactory.Instance);//设置photon我们使用哪个日志插件
				XmlConfigurator.ConfigureAndWatch(configFileInfo);//让log4net这个插件读取配置文件
			}

			log.Info("Start Server Succecel!");//最后利用log对象就可以输出了
		}
	}
}
