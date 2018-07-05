using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace Yeelight
{
    class YeelightBulb
    {
        private TcpClient socket = new TcpClient();
        public string ipAdress { get; private set; }
        public enum state
        {
            Unknow,
            On,
            Off
        };
        public state lightState = state.Unknow;

        public YeelightBulb(string ipAdress)
        {
            this.ipAdress = ipAdress;
        }

        private int _Dim;
        public int Dim
        {
            get { return _Dim; }
            set
            {
                if (value == 0)
                    value++;
                _Dim = value;
                
                if (this.lightState == state.Off)  this.TurnOn();

                sendCommand("{\"id\":1,\"method\":\"set_bright\",\"params\":[" + _Dim.ToString() + ", \"smooth\",500]}\r\n");
            }
        }

        public void TurnOff()
        {
            sendCommand("{\"id\":1,\"method\":\"set_power\",\"params\":[\"off\", \"smooth\",500]}\r\n");
            lightState = state.Off;
        }

        public void TurnOn()
        {
            sendCommand("{\"id\":1,\"method\":\"set_power\",\"params\":[\"on\", \"smooth\",500]}\r\n");
            lightState = state.On;
        }
        private void sendCommand(string cmd)
        {
            try
            {
                socket = new TcpClient();
                socket.SendTimeout = 1000;
                socket.NoDelay = true;
                socket.Connect(ipAdress, 55443);
                socket.Client.Send(Encoding.ASCII.GetBytes(cmd));
            }
            catch (Exception e)
            {
                throw new TimeoutException($"Yeelight on ip: {ipAdress} couldn't be found");
            }
            socket.Close();
        }

    }
}
