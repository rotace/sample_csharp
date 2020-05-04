using EchoCommon;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

namespace EchoServer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public StateViewModel ServerStateViewModel { set; get; }
        internal TcpMessageServer tcpMessageServer;
        internal UdpMessageServer udpMessageServer;

        public MainWindow()
        {
            //インスタンス生成
            ServerStateViewModel = new StateViewModel();
            
            //GUI構築
            InitializeComponent();
            this.DataContext = ServerStateViewModel;

            //TCPサーバ起動
            tcpMessageServer = new TcpMessageServer(ServerStateViewModel);
            //UDPサーバ起動
            udpMessageServer = new UdpMessageServer(ServerStateViewModel);
        }
    }
}
