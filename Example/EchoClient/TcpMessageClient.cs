using EchoCommon;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace EchoClient
{
    class TcpMessageClient
    {
        //状態管理及びUI関連
        private StateViewModel ClientStateViewModel { set; get; }

        //ソケット通信関連
        private readonly static string SERVER_HOST = "127.0.0.1";
        private readonly static string CLIENT_HOST = "127.0.0.1";
        private readonly static int SERVER_PORT = 50000;
        private readonly static int CLIENT_PORT = 50010;
        private TcpClient client = null;
        private NetworkStream stream = null;

        //コンストラクタ
        public TcpMessageClient(StateViewModel clientStateViewModel)
        {
            //インスタンス生成
            client = new TcpClient();

            //ビューモデル格納
            ClientStateViewModel = clientStateViewModel;
        }

        public bool ToggleConnection()
        {
            //通信設定
            IPEndPoint ipServerPoint = new IPEndPoint(IPAddress.Parse(SERVER_HOST), SERVER_PORT);
            IPEndPoint ipClientPoint = new IPEndPoint(IPAddress.Parse(CLIENT_HOST), CLIENT_PORT);

            try
            {
                if(client.Connected)
                {
                    //切断
                    client.Close();
                    client = new TcpClient();
                }
                else
                {
                    //接続要求
                    client.Connect(ipServerPoint);
                    stream = client.GetStream();
                }
            }
            catch (SocketException)
            {
                //接続を拒否された場合、等
                ClientStateViewModel.DispMessage("Connection Error");
                return false;
            }
            catch (ObjectDisposedException)
            {
                //何らかの原因により既に切断され、Disposeされたclientで再接続を試みた場合
                ClientStateViewModel.DispMessage("Disposed Error");
                client = new TcpClient();
                ClientStateViewModel.ConnectionCommand = "Connect";
                return false;
            }

            if(client.Connected)
            {
                ClientStateViewModel.DispMessage("Connected");
                ClientStateViewModel.ConnectionCommand = "Disconnect";
            }
            else
            {
                ClientStateViewModel.DispMessage("Disconnected");
                ClientStateViewModel.ConnectionCommand = "Connect";
            }

            return true;
        }

        public void SendCommand(string message)
        {
            if(client.Connected)
            {
                byte[] bytes = Encoding.ASCII.GetBytes(message);

                stream.Write(bytes, 0, bytes.Length);
                stream.Flush();
                ClientStateViewModel.DispMessage("TCP:Send:" + Encoding.ASCII.GetString(bytes));

                //System.Diagnostics.Trace.WriteLine(Encoding.ASCII.GetString(bytes));
            }
            else
            {
                ClientStateViewModel.DispMessage("Not Connected");
            }

        }

        public async Task SendAndRecvMessageAsync(string message)
        {
            if(client.Connected)
            {
                byte[] bytes = Encoding.ASCII.GetBytes(message);

                stream.Write(bytes, 0, bytes.Length);
                await stream.FlushAsync();
                ClientStateViewModel.DispMessage("TCP:Send:" + Encoding.ASCII.GetString(bytes));

                //別スレッドでメッセージを待受
                bytes = new byte[256];
                int len = await stream.ReadAsync(bytes, 0, bytes.Length);
                ClientStateViewModel.DispMessage("TCP:Recv:" + Encoding.ASCII.GetString(bytes));

                //System.Diagnostics.Trace.WriteLine(Encoding.ASCII.GetString(bytes));
            }
            else
            {
                ClientStateViewModel.DispMessage("Not Connected");
            }
        }
    }
}
