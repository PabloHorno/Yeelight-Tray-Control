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
        private TcpClient Socket = new TcpClient();
        private String ipAdress = "";
        
        public Yeelight(string ipAdress)
        {
            this.ipAdress = ipAdress;
        }
        private int _brillo;
        public int Brillo {
            get { return _brillo; }

            set {
                if (value == 0)
                    value++;

                Socket = new TcpClient();
                Socket.Connect(ipAdress, 55443);
                Socket.Client.Send(Encoding.ASCII.GetBytes("{\"id\":1,\"method\":\"set_bright\",\"params\":[" + _brillo.ToString() + ", \"smooth\",500]}\r\n"));
                Socket.Close();
            }
        }

        public void Apagar()
        {
        };
        public void Encender()
        {
        };

    }
}
