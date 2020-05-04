using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace EchoCommon
{
    /// <summary>
    /// 状態ビューモデル
    /// 
    /// このビューモデルは、モデルとしての役割を兼任する。
    /// 参考：https://www.atmarkit.co.jp/ait/articles/1011/09/news102_3.html
    /// 
    /// ビューモデルは、モデルとビューを繋げるための接着剤の役割として、以下を果たす。
    /// (1) モデルをWPF向けにラッピング（モデルの状態変化をビューに通知するためのINotifyPropertyChangedの実装、等）
    /// (2) ビューから状態を分離
    /// 
    /// モデルは、TCP/UDP通信で取り扱う情報の、Server/Clientが保持する内部状態を表す他、
    /// 必要に応じてビジネスロジックも記述する。
    /// 
    /// </summary>
    public partial class StateViewModel : INotifyPropertyChanged
    {
        //ビューに状態変更の通知を行うハンドラ
        public event PropertyChangedEventHandler PropertyChanged;

        //Textページ関連の定義
        public ObservableCollection<string> MessageList { get; set; } = new ObservableCollection<string>();
        public string Message { get; set; }

        //TCPコマンドの定義
        public ObservableCollection<ComboFormat> TcpCmdMsgDataAList { get; set; } = new ObservableCollection<ComboFormat>
        {
            new ComboFormat { Bits = 32, Data = "0x00000000", Info = "Hello, " },
            new ComboFormat { Bits = 32, Data = "0x00000001", Info = "Goodbye, " }
        };
        public ObservableCollection<ComboFormat> TcpCmdMsgDataBList { get; set; } = new ObservableCollection<ComboFormat>
        {
            new ComboFormat { Bits = 32, Data = "0x00000000", Info = "Mark!" },
            new ComboFormat { Bits = 32, Data = "0x00000001", Info = "Nancy!" }
        };

        public ComboFormat TcpCmdMsgDataA { get; set; }
        public ComboFormat TcpCmdMsgDataB { get; set; }

        //UDPステータスの定義
        public ObservableCollection<ComboFormat> UdpAlarmList { get; set; } = new ObservableCollection<ComboFormat>
        {
            new ComboFormat { Bits = 1, Data = "0x00", Info = "Good"},
            new ComboFormat { Bits = 1, Data = "0x01", Info = "Bad"}
        };

        //UDPステータスはEchoClientで受信時にモデルの状態を変更するため、
        //セッターが呼ばれたタイミングでモデルの状態変更をPropertyChangedで通知する必要がある。
        private ComboFormat _udp_alm_a1;
        private ComboFormat _udp_alm_a2;
        private ComboFormat _udp_alm_b1;
        private ComboFormat _udp_alm_b2;
        public ComboFormat UdpAlarmA1
        {
            get { return _udp_alm_a1; }
            set
            {
                _udp_alm_a1 = value;
                //モデルの状態変更をビューに反映する
                PropertyChanged(this, new PropertyChangedEventArgs("UdpAlarmA1"));
            }
        }
        public ComboFormat UdpAlarmA2
        {
            get { return _udp_alm_a2; }
            set
            {
                _udp_alm_a2 = value;
                //モデルの状態変更をビューに反映する
                PropertyChanged(this, new PropertyChangedEventArgs("UdpAlarmA2"));
            }
        }
        public ComboFormat UdpAlarmB1
        {
            get { return _udp_alm_b1; }
            set
            {
                _udp_alm_b1 = value;
                //モデルの状態変更をビューに反映する
                PropertyChanged(this, new PropertyChangedEventArgs("UdpAlarmB1"));
            }
        }
        public ComboFormat UdpAlarmB2
        {
            get { return _udp_alm_b2; }
            set
            {
                _udp_alm_b2 = value;
                //モデルの状態変更をビューに反映する
                PropertyChanged(this, new PropertyChangedEventArgs("UdpAlarmB2"));
            }
        }

        //接続状態
        //セッターが呼ばれたタイミングでモデルの状態変更をPropertyChangedで通知する必要がある。
        private string _connection_command = "Connect";
        public string ConnectionCommand
        {
            get { return _connection_command;  }
            set
            {
                _connection_command = value;
                //モデルの状態変更をビューに反映する
                PropertyChanged(this, new PropertyChangedEventArgs("ConnectionCommand"));
            }
        }


        /// <summary>
        /// メッセージの表示
        /// </summary>
        /// <param name="str"></param>
        public void DispMessage(string str)
        {
            MessageList.Add(str);

            if(MessageList.Count > 10)
            {
                MessageList.RemoveAt(0);
            }
        }
    }

    public partial class ComboFormat
    {
        public int Bits { get; set; }
        public string Data { get; set; }
        public string Info { set; get; }
    }
}
