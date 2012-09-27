using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading;
using System.Net.Sockets;


namespace NetZ
{
    public enum MessageHeaders
    {
        REGULAR = 0,
        USERSNAMES,
        CONNECT,
        DISCONNECT,
        SYSTEM,
        PULSE,
        COORD,
        STFU,
        ALLCLEAR,
        ERROR
    }
    public class Multicast
    {
        #region Variables
        public string m_Port;
        public string m_MultiCastEndpoint;

        List<string> m_UserNamesCache = new List<string>();
        

        Thread m_recieveThread;
        Thread m_SendThread;
        Thread m_PulseThread;

        Socket m_Socket;

        public List<string> m_ReceiveQueue = new List<string>();
        public List<string> m_SendQueue = new List<string>();

        bool m_readyToReceive;
        public bool isAllowedToSend;
        public bool isAllowedToReceive;

        #endregion

        #region Constructor
        public Multicast(string port = "1337", string multiCastEndpoint = "224.100.0.1")
        {
            m_MultiCastEndpoint = multiCastEndpoint;
            m_Port = port;

        }

        public void Start(bool doSend = true, bool doReceive = true)
        {
            m_readyToReceive = false;

            m_Socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            isAllowedToReceive = true;
            m_recieveThread = new Thread(new ThreadStart(ReceiveMessage));
            m_recieveThread.IsBackground = true;
            if(doReceive)
            m_recieveThread.Start();

            isAllowedToSend = true;
            m_SendThread = new Thread(new ThreadStart(SendMessage));
            m_SendThread.IsBackground = true;
            if(doSend)
            m_SendThread.Start();
        }
        #endregion

        #region Pulse
        struct PulseParams
        {
            public MessageHeaders type;
            public string ToPulse;
        }

        public void StartPulse(MessageHeaders type, string ToPulse)
        {
            PulseParams pp = new PulseParams();
            pp.type = type;
            pp.ToPulse = ToPulse;

            m_PulseThread = new Thread(new ParameterizedThreadStart(Pulse));
            m_PulseThread.IsBackground = true;
            m_PulseThread.Start(pp);
        }

        void Pulse(object pp)
        {
            if (m_readyToReceive)
            {
                while (true)
                {
                    m_SendQueue.Add((int)((PulseParams)pp).type + ((PulseParams)pp).ToPulse);
                    Thread.Sleep(1000);
                }
            }
        }
        #endregion

        #region Send
        void SendMessage()
        {
            IPEndPoint iep = new IPEndPoint(IPAddress.Parse(m_MultiCastEndpoint), int.Parse(m_Port));
            byte[] data = new byte[1024];

            while (true)
            {
                if (isAllowedToSend)
                {
                    if (m_readyToReceive)
                    {
                        if (m_SendQueue.Count > 0)
                        {
                            try
                            {
                                data = Encoding.ASCII.GetBytes(m_SendQueue[0]);
                                m_Socket.SendTo(data, iep);
                                m_SendQueue.RemoveAt(0);
                            }
                            catch (System.Exception ex)
                            {

                            }

                        }
                    }
                }
            }
        }
        #endregion

        #region Receive
        public void ReceiveMessage()
        {
            IPEndPoint iep = new IPEndPoint(IPAddress.Any, int.Parse(m_Port));
            EndPoint ep = (EndPoint)iep;
            m_Socket.Bind(iep);
            m_Socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, new MulticastOption(IPAddress.Parse(m_MultiCastEndpoint)));
            byte[] data = new byte[1024];
            int recv;
            string stringData;

            m_readyToReceive = true;

            while (true)
            {
                if (isAllowedToReceive)
                {
                    try
                    {
                        recv = m_Socket.ReceiveFrom(data, ref ep);
                        stringData = Encoding.ASCII.GetString(data, 0, recv);
                        m_ReceiveQueue.Add(stringData);
                    }
                    catch (System.Exception ex)
                    {

                    }
                }
            }
        }
        #endregion
    }
}
