using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

/* =======================================================================
* 创建时间：2015/12/9 16:20:27
* 作者：sweet
* Framework: 4.5
* ========================================================================
*/

namespace sweet.framework.Utility.Protocol
{
    public class SocketClient : IDisposable
    {
        private const int BUFF_SIZE = 20480;

        private Socket _socket = null;
        private Thread _receiveThread = null;
        private Encoding _encoding = null;

        /// <summary>
        /// 接收到服务端消息
        /// </summary>
        public event Action<string> ReceiveEvent;

        public SocketClient()
        {
            this._encoding = Encoding.UTF8;
        }

        public void Connect(string host, int port)
        {
            this._socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            this._socket.Connect(host, port);

            //初始化接收线程
            if (_receiveThread != null) { _receiveThread.Abort(); }

            _receiveThread = new Thread(Receive);
            _receiveThread.IsBackground = true;
            _receiveThread.Start();
        }

        public int Send(string msg)
        {
            if (!this._socket.Connected)
            {
                throw new Exception("can not send msg before socket connected.");
            }

            return this._socket.Send(this._encoding.GetBytes(msg));
        }

        public void Receive()
        {
            byte[] buffer = new byte[BUFF_SIZE];

            while (true)
            {
                if (!this._socket.Connected)
                {
                    throw new Exception("can not receive msg before socket connected.");
                }

                #region 逐字节读取

                //const int setp = 1;
                //int current = 0;
                //int len = _socket.Receive(buffer, 0, setp, SocketFlags.None);
                //current += len;

                //while (len > 0)
                //{
                //    len = _socket.Receive(buffer, current, setp, SocketFlags.None);

                //    if (buffer[current] == '\r' || buffer[current] == '\n')
                //    {
                //        string msg = _encoding.GetString(buffer, 0, current);

                //        LogUtility.GetInstance().Info("receive: {0}", msg);
                //        if (ReceiveEvent != null) { ReceiveEvent(msg); }

                //        break;
                //    }

                //    current += len;
                //}

                #endregion 逐字节读取

                int len = _socket.Receive(buffer, 0, buffer.Length, SocketFlags.None);

                string msg = _encoding.GetString(buffer, 0, len);

                LogUtility.GetInstance().Info("receive: {0}", msg);

                if (ReceiveEvent != null) { ReceiveEvent(msg); }
            }
        }

        public void Dispose()
        {
            if (this._socket != null)
            {
                this._socket.Dispose();
            }
        }
    }
}