using EchoCommon;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace EchoServer
{
    class UdpMessageServer
    {
        //状態管理及びUI関連
        private StateViewModel ServerStateViewModel { set; get; }

        //ソケット通信関連
        private readonly static string SERVER_HOST = "127.0.0.1";
        private readonly static string CLIENT_HOST = "127.0.0.1";
        private readonly static int SERVER_PORT = 50001;
        private readonly static int CLIENT_PORT = 50011;
        private readonly DispatcherTimer timer = null;

        //コンストラクタ
        public UdpMessageServer(StateViewModel serverStateViewModel)
        {
            //ビューモデル格納
            ServerStateViewModel = serverStateViewModel;

            //タイマー（WPFのタイマーを利用）
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += new EventHandler(SendStatusMessage);
            timer.Start();
        }

        private void SendStatusMessage(object sender, EventArgs e)
        {
            //System.Diagnostics.Trace.WriteLine("SendStatusMessage");

            //通信設定
            IPEndPoint ipServerPoint = new IPEndPoint(IPAddress.Parse(SERVER_HOST), SERVER_PORT);
            IPEndPoint ipClientPoint = new IPEndPoint(IPAddress.Parse(CLIENT_HOST), CLIENT_PORT);

            //自分ポート確保（usingで関数終了と同時に開放）
            using var server = new UdpClient(ipServerPoint);
            //相手ポート接続
            server.Connect(ipClientPoint);

            //メッセージ作成
            byte[] bytes = Encoding.ASCII.GetBytes(
                " A1:" + ServerStateViewModel.UdpAlarmA1.Info +
                " A2:" + ServerStateViewModel.UdpAlarmA2.Info +
                " B1:" + ServerStateViewModel.UdpAlarmB1.Info +
                " B2:" + ServerStateViewModel.UdpAlarmB2.Info
                );

            //送信
            server.Send(bytes, bytes.Length);

            //ディスプレイ表示
            ServerStateViewModel.DispMessage("UDP:Send:" + Encoding.ASCII.GetString(bytes));
        }
    }
}
