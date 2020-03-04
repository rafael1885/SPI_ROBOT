using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace SPI_ROBOT
{
    public partial class VisualizarUsuario : Form
    {
        private const int SW_MAXIMIZE = 3;
        private const int SW_SHOWMINIMIZED = 2;
        private const int SW_MINIMIZE = 6;
        private const int SW_HIDE = 0;
        private const int SW_RESTORE = 9;
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("User32.dll")]                                                                                                   //*Verificar
        static extern int SetForegroundWindow(IntPtr point);

        private bool offline;
        public bool OffLine
        {
            get { return offline; }
            set { offline = value; }
        }
        public VisualizarUsuario()
        {
            InitializeComponent();

            lb_usuario.Text = File.ReadLines("user.dll").Skip(0).Take(1).First();
            string Id = File.ReadLines("user.dll").Skip(1).Take(1).First();


            pb_foto.BackgroundImage = Image.FromFile(Id + ".PNG");
            pb_foto.BackgroundImageLayout = ImageLayout.Stretch;


        }
        public void indica_operacao()
        {
            if (!offline)
            {
                ModoOperacao.Text = "Online";
                ModoOperacao.BackColor = Color.Blue;
            }
            else
            {
                ModoOperacao.Text = "Offline";
                ModoOperacao.BackColor = Color.Red;
            }
        }
        private void Bt_voltar_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
