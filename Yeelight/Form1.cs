﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms.Design;

namespace Yeelight
{
    public partial class Form1 : Form
    {
        const string ipAdress = "192.168.1.4";
        private bool estado = false;
        private int _brillo = 0;
        public int Brillo {
            get { return _brillo; }

            set
            {
                if (value == 0)
                    value++;
                _brillo = value;

                TcpClient socket = new TcpClient();
                socket.Connect(ipAdress, 55443);
                socket.Client.Send(Encoding.ASCII.GetBytes("{\"id\":1,\"method\":\"set_bright\",\"params\":[" + _brillo.ToString() + ", \"smooth\",500]}\r\n"));
                socket.Close();
            }
        }
        public Form1()
        {
            InitializeComponent();
            this.Resize += Form1_Resize;
            this.Luz.MouseClick += NotifyIcon1_MouseClick;
            Luz.ContextMenuStrip = contextMenuStrip1;
            this.porcentajeToolStripMenuItem.DragLeave += PorcentajeToolStripMenuItem_DragLeave;
            ApagarBombilla();
            this.Hide();
            this.WindowState = FormWindowState.Minimized;
        }
        ~Form1()
        {
            ApagarBombilla();
        }

        private void PorcentajeToolStripMenuItem_DragLeave(object sender, EventArgs e)
        {
            MessageBox.Show((sender as TrackBar).Value.ToString());
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == this.WindowState)
            {
                Luz.Visible = true;
                Luz.ShowBalloonTip(100, "Luz"," ", ToolTipIcon.Info);
                this.Hide();
            }
            else if (FormWindowState.Normal == this.WindowState)
            {
                Luz.Visible = false;
            }
        }

        private void ApagarBombilla()
        {
            TcpClient socket = new TcpClient();
            socket.Connect(ipAdress, 55443);
            socket.Client.Send(Encoding.ASCII.GetBytes("{\"id\":1,\"method\":\"set_power\",\"params\":[\"off\", \"smooth\",500]}\r\n"));
            socket.Close();
        }
        private void EncenderBombilla()
        {
            TcpClient socket = new TcpClient();
            socket.Connect(ipAdress, 55443);
            socket.Client.Send(Encoding.ASCII.GetBytes("{\"id\":1,\"method\":\"set_power\",\"params\":[\"on\", \"smooth\",500]}\r\n"));
            socket.Close();
        }
        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (estado)
            {
                Luz.Icon = new Icon("icon_off.ico");
                Luz.Text = "Luz apagada";
                ApagarBombilla();
            }
            else
            {
                Luz.Icon = new Icon("icon_on.ico");
                Luz.Text = "Luz encendida";
                EncenderBombilla();
            }
            estado = !estado;
        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void NotifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Right)
            {
                Luz.ContextMenuStrip.Show();
            }
        }

        [ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.MenuStrip | ToolStripItemDesignerAvailability.ContextMenuStrip)]
        public class TrackBarMenuItem : ToolStripControlHost
        {
            public TrackBar trackBar;

            public TrackBarMenuItem() : base(new TrackBar())
            {
                this.trackBar = this.Control as TrackBar;
            }
        }

        private void porcentajeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Brillo = (sender as TrackBarMenuItem).trackBar.Value * 10;
        }
    }
}
