using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace RegularCapture
{
    public partial class Form1 : Form
    {
        
        [DllImport("user32.dll")]
        static extern bool RegisterHotKey(IntPtr hWnd, int id, int modifiers, Keys vk);
        [DllImport("user32.dll")]
        static extern bool UnregisterHotKey(IntPtr hWnd, int id);
        
        public Form1()
        {

            InitializeComponent();
        }
        
        private void regularCapIco_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if(this.WindowState == FormWindowState.Minimized)
            {
                this.Show();
                this.WindowState = FormWindowState.Normal;
                regularCapIco.Visible = false;
                this.ShowInTaskbar = true;
            }
        }
        #region
        private void Form1_Resize(object sender, EventArgs e)
        {
            if(this.WindowState == FormWindowState.Minimized)
            {
                this.Hide();
                this.ShowInTaskbar = false;
                this.regularCapIco.Visible = true;
            }
        }
        #endregion
        
        private void button1_Click(object sender, EventArgs e)
        {

            saveFolder.ShowDialog();
            label2.Text = "Save in " + saveFolder.SelectedPath;
            label2.Visible = true;
            
        }
        
        private void ScreenCapture()
        {
            Bitmap img = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            Graphics graph = Graphics.FromImage(img);
            graph.CopyFromScreen(0, 0, 0, 0, new Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height));

            //save image
            string fileName = saveFolder.SelectedPath + "\\" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".png";
            label1.Text = "Save as " + fileName;
            img.Save(fileName, System.Drawing.Imaging.ImageFormat.Png);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ScreenCapture();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            label2.Text = "Save in " + saveFolder.SelectedPath;
        }

        private void Form1_Close(object sender, FormClosingEventArgs e)
        {
        }
        
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            const int WM_HOTKEY = 0x0312;
            const int WM_CREATE = 0x1;
            const int WM_DESTORY = 0x2;
            switch (m.Msg)
            {
                case WM_CREATE:
                    //hotkey regist
                    RegisterHotKey(this.Handle, 233, (0x0002 | 0x0001), Keys.C);
                    break;
                case WM_DESTORY:
                    //hotkey unregist
                    UnregisterHotKey(this.Handle, 233);
                    break;
                case WM_HOTKEY:
                    if (m.WParam.ToInt32() == 233)
                        ScreenCapture();
                    break;
                default:break;
            }
        }
        
    }
}
