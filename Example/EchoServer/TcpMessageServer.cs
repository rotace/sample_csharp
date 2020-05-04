using EchoCommon;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EchoServer
{
    class TcpMessageServer
    {
        //状態管理及びUI関連
        private StateViewModel ServerStateViewModel { set; get; }

        //ソケット通信関連
        private readonly static string SERVER_HOST = "127.0.0.1";
        private readonly static string CLIENT_HOST = "127.0.0.1";
        private readonly static int SERVER_PORT = 50000;
        private readonly static int CLIENT_PORT = 50010;
        private readonly TcpListener listener = null;

        //コンストラクタ
        public TcpMessageServer( StateViewModel serverStateViewModel )
        {
            //ビューモデル格納
            ServerStateViewModel = serverStateViewModel;

            //通信設定
            IPEndPoint ipendpoint = new IPEndPoint(IPAddress.Parse(SERVER_HOST), SERVER_PORT);
            listener = new TcpListener(ipendpoint);

            //スレッド開始（返り値を破棄してコンストラクタを終了）
            _ = StartThreadAsync();

            //System.Diagnostics.Trace.WriteLine("TcpMessageServer Constructor Finish");
        }

        public async Task StartThreadAsync()
        {
            //クライアント受付開始
            listener.Start();

            while (true)
            {
                //別スレッドでクライアント接続要求を待受
                using var server = await listener.AcceptTcpClientAsync();
                using var binStream = server.GetStream();
                using var txtReader = new StreamReader(binStream, Encoding.UTF8);
                using var txtWriter = new StreamWriter(binStream, Encoding.UTF8);

                while (true)
                {
                    if (true)
                    {
                        //バイナリで処理する場合

                        //別スレッドでメッセージを待受
                        byte[] bytes = new byte[256];
                        int len = await binStream.ReadAsync(bytes, 0, bytes.Length);

                        if (len == 0)
                        {
                            break;
                        }

                        binStream.Write(bytes, 0, len);
                        await binStream.FlushAsync();

                        ServerStateViewModel.DispMessage("TCP:Recv:" + Encoding.ASCII.GetString(bytes));
                    }
                    else
                    {
                        //文字列で処理する場合

                        //別スレッドでメッセージを待受（注意：受信完了には改行コードが必要）
                        string message = await txtReader.ReadLineAsync();

                        if (message == null)
                        {
                            break;
                        }

                        txtWriter.WriteLine(message);
                        await txtWriter.FlushAsync();
                    }
                }
            }
        }
    }
}
