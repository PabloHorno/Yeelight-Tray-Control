using System;
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
using System.Threading;

namespace Yeelight
{
    public partial class Form1 : Form
    {
        Yeelight yeelight;
        public Form1()
        {
            InitializeComponent();
            this.Resize += Form1_Resize;
            this.Luz.MouseClick += NotifyIcon1_MouseClick;
            Luz.ContextMenuStrip = contextMenuStrip1;
            this.porcentajeToolStripMenuItem.trackBar.Scroll += onTrackBarScroll;
            this.WindowState = FormWindowState.Minimized;
            this.ShowInTaskbar = false;
            this.Hide();
            yeelight = new Yeelight(Settings.IpAddress);            
        }
        ~Form1()
        {
            yeelight.TurnOff();
        }

        private void onTrackBarScroll(object sender, EventArgs e)
        {
            yeelight.Dim = (sender as TrackBar).Value * 10;

            Luz.Icon = new Icon("Icons/icon_on.ico");
            Luz.Text = "Light on";
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == this.WindowState)
            {
                Luz.Visible = true;
                Luz.ShowBalloonTip(100, "Yeelight tray control", $"ipAdress: {yeelight.ipAdress}", ToolTipIcon.Info);
                this.Hide();
            }
            else if (FormWindowState.Normal == this.WindowState)
            {
                Luz.Visible = false;
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var thread = new Thread(() => AnimIcon(Luz));
            thread.Start();
            Luz.Text = "Comunication in progress";
            try
            {

                if (yeelight.lightState == Yeelight.state.On)
                {
                    yeelight.TurnOff();
                    Luz.Icon = new Icon("Icons/icon_off.ico");
                    Luz.Text = "Light off";
                }
                else
                {
                    yeelight.TurnOn();
                    Luz.Icon = new Icon("Icons/icon_on.ico");
                    Luz.Text = "Light on";
                }
            }

            catch (Exception ex)
            {
                thread.Abort();
                Luz.Icon = new Icon("Icons/sync_error.ico");
                Luz.Text = "Error";
                Luz.ShowBalloonTip(100, "Yeelight Not Found", ex.Message, ToolTipIcon.Info);
            }
            if(thread.IsAlive)
                thread.Abort();
        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void NotifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
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
        private static void AnimIcon(NotifyIcon Luz)
        {
            Bitmap icon = new Bitmap("Icons/loading.ico");
            try
            {
                while (true)
                {
                    icon.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    

                    Luz.Icon = Icon.FromHandle(icon.GetHicon());
                    Thread.Sleep(100);
                }
            }
            catch (ThreadAbortException abortException)
            {
                Console.WriteLine(abortException.Message);
            }
        }
    }
}
