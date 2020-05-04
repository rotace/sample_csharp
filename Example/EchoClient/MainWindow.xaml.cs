using EchoCommon;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EchoClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public StateViewModel ClientStateViewModel { set; get; }
        internal TcpMessageClient tcpMessageClient;
        internal UdpMessageClient udpMessageClient;

        public MainWindow()
        {
            //インスタンス生成
            ClientStateViewModel = new StateViewModel();

            //GUI構築
            InitializeComponent();
            this.DataContext = ClientStateViewModel;

            //TCPクライアント起動
            tcpMessageClient = new TcpMessageClient(ClientStateViewModel);
            //UDPクライアント起動
            udpMessageClient = new UdpMessageClient(ClientStateViewModel);
        }

        private void ConnectBtn_Click(object sender, RoutedEventArgs e)
        {
            tcpMessageClient.ToggleConnection();
        }

        private void TcpSetMsgBtn_Click(object sender, RoutedEventArgs e)
        {
            tcpMessageClient.SendCommand("SetMessage: " + ClientStateViewModel.TcpCmdMsgDataA.Info + ClientStateViewModel.TcpCmdMsgDataB.Info);
        }

        private void TcpGetMsgBtn_Click(object sender, RoutedEventArgs e)
        {
            tcpMessageClient.SendCommand("GetMessage");
        }

        private async void MessageSendBtn_Click(object sender, RoutedEventArgs e)
        {
            //送信開始（非同期）
            //await TcpMsgClient.SendAndRecvMessageAsync(ClientStateViewModel.Message);

            //送信開始（同期）
            tcpMessageClient.SendCommand(ClientStateViewModel.Message);
        }
    }
}
