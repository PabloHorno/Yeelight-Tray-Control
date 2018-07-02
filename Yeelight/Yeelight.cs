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

                socket = new TcpClient();
                socket.Connect(ipAdress, 55443);
                socket.Client.Send(Encoding.ASCII.GetBytes("{\"id\":1,\"method\":\"set_bright\",\"params\":[" + _Dim.ToString() + ", \"smooth\",500]}\r\n"));
                socket.Close();
            }
        }

        public void TurnOff()
        {
            try
            {
                socket = new TcpClient();
                socket.Connect(ipAdress, 55443);
                socket.Client.Send(Encoding.ASCII.GetBytes("{\"id\":1,\"method\":\"set_power\",\"params\":[\"off\", \"smooth\",500]}\r\n"));
                lightState = state.Off;

            }
            catch (Exception e)
            {
                throw new TimeoutException($"Yeelight on ip: {ipAdress} couldn't be found");
            }

            socket.Close();
        }

        public void TurnOn()
        {
            try
            {
                socket = new TcpClient();
                socket.Connect(ipAdress, 55443);
                socket.Client.Send(Encoding.ASCII.GetBytes("{\"id\":1,\"method\":\"set_power\",\"params\":[\"on\", \"smooth\",500]}\r\n"));
                lightState = state.On;

            }
            catch (Exception e)
            {
                throw new TimeoutException($"Yeelight on ip: {ipAdress} couldn't be found");
            }
            socket.Close();
        }

    }
}
