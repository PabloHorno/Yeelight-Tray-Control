using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace Yeelight
{
    class Yeelight
    {
        private TcpClient socket = new TcpClient();
        private String ipAdress = "";
        public bool state = false;

        public Yeelight(string ipAdress, bool defaultState = false)
        {
            this.ipAdress = ipAdress;
            if (defaultState)
                this.TurnOn();
            else
                this.TurnOff();
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

                socket = new TcpClient();
                socket.Connect(ipAdress, 55443);
                socket.Client.Send(Encoding.ASCII.GetBytes("{\"id\":1,\"method\":\"set_bright\",\"params\":[" + _Dim.ToString() + ", \"smooth\",500]}\r\n"));
                socket.Close();
            }
        }

        public void TurnOff()
        {
            socket = new TcpClient();
            try
            {
                socket.Connect(ipAdress, 55443);
                socket.Client.Send(Encoding.ASCII.GetBytes("{\"id\":1,\"method\":\"set_power\",\"params\":[\"off\", \"smooth\",500]}\r\n"));
                socket.Close();
                state = false;

            }
            catch (Exception e)
            { }
        }

        public void TurnOn()
        {
            try
            {
                socket = new TcpClient();
                socket.Connect(ipAdress, 55443);
                socket.Client.Send(Encoding.ASCII.GetBytes("{\"id\":1,\"method\":\"set_power\",\"params\":[\"on\", \"smooth\",500]}\r\n"));
                socket.Close();
                state = true;

            }
            catch (Exception e)
            { }
        }

    }
}
