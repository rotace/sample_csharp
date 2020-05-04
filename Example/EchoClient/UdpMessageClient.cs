using EchoCommon;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace EchoClient
{
    class UdpMessageClient
    {
        //状態管理及びUI関連
        private StateViewModel ClientStateViewModel { set; get; }

        //ソケット通信関連
        private readonly static string SERVER_HOST = "127.0.0.1";
        private readonly static string CLIENT_HOST = "127.0.0.1";
        private readonly static int SERVER_PORT = 50001;
        private readonly static int CLIENT_PORT = 50011;

        //コンストラクタ
        public UdpMessageClient(StateViewModel clientStateViewModel)
        {
            //ビューモデル格納
            ClientStateViewModel = clientStateViewModel;

            //スレッド開始（返り値を破棄してコンストラクタを終了）
            _ = StartThreadAsync();
        }

        public async Task StartThreadAsync()
        {
            //通信設定
            IPEndPoint ipServerPoint = new IPEndPoint(IPAddress.Parse(SERVER_HOST), SERVER_PORT);
            IPEndPoint ipClientPoint = new IPEndPoint(IPAddress.Parse(CLIENT_HOST), CLIENT_PORT);

            //自分ポート確保（usingで関数終了と同時に開放）
            using var client = new UdpClient(ipClientPoint);
            //相手ポート接続
            client.Connect(ipServerPoint);

            while (true)
            {
                //別スレッドでサーバーからのメッセージを待受
                UdpReceiveResult message = await client.ReceiveAsync();
                ClientStateViewModel.DispMessage("UDP:Recv:" + Encoding.ASCII.GetString(message.Buffer));

                //System.Diagnostics.Trace.WriteLine(Encoding.ASCII.GetString(message.Buffer));
            }
        }
    }
}
