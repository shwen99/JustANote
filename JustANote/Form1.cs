using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace JustANote
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            textBox1.Select(0,0);
            btnClose.Hide();
            this.Close();
        }

        private void HideButtons(object sender, EventArgs e)
        {
            btnClose.Text = "";
        }

        private void ShowButtons(object sender, EventArgs e)
        {
            btnClose.Text = "×";
        }

        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);

        private void pnlCaption_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture(); //释放鼠标捕捉
                //发送左键点击的消息至该窗体(标题栏)
                SendMessage(Handle, 0xA1, 0x02, 0);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            label1.Font = textBox1.Font;

            HideButtons(null, null);

            var location = Settings.Default.NoteWindow;

            if (location.Width == 0)
            {
                Left = Screen.FromControl(this).WorkingArea.Width - Width - 30;
                Top = 30;
            }
            else
            {
                SetBounds(location.Left, location.Top, location.Width, location.Height);
            }

            textBox1.Text = Settings.Default.Note;
            textBox1.SelectionStart = 65535;

            var wallfile = Settings.Default.OrgFile;

            if (!string.IsNullOrEmpty(wallfile))
            {
                wallfile = GetWallpaper();
                SetWallpaper(wallfile);
            }

        }

        private void Form1_Deactivate(object sender, EventArgs e)
        {
            Save();
        }

        private void Save()
        {
            using (var bmp = new Bitmap(Width, Height, PixelFormat.Format32bppArgb))
            {
                var clientLocation = PointToScreen(pnlCaption.Location);

                DrawToBitmap(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height));
                var screen = Screen.PrimaryScreen.Bounds;
                var wallfile = Settings.Default.OrgFile;
                var notefile = Path.GetFullPath(".\\note.png");

                if (string.IsNullOrEmpty(wallfile))
                {
                    wallfile = GetWallpaper();
                    Settings.Default.OrgFile = wallfile;
                }

                using (var bmp2 = new Bitmap(screen.Width, screen.Height))
                using (var wallpaper = Image.FromFile(wallfile))
                using (var g = Graphics.FromImage(bmp2))
                {
                    g.DrawImage(wallpaper, screen, new Rectangle(0, 0, wallpaper.Width, wallpaper.Height), GraphicsUnit.Pixel);
                    g.DrawImage(bmp, new Rectangle(clientLocation, ClientSize), new Rectangle(clientLocation.X - Location.X, clientLocation.Y - Location.Y, ClientSize.Width, ClientSize.Height), GraphicsUnit.Pixel);
                    bmp2.Save(notefile, ImageFormat.Png);
                    SetWallpaper(notefile);
                }
            }

            Settings.Default.NoteWindow = Bounds;
            Settings.Default.Note = textBox1.Text;
            Settings.Default.Save();
        }

        static readonly Guid CLSID_ActiveDesktop = new Guid("{75048700-EF1F-11D0-9888-006097DEACF9}");

        public static IActiveDesktop GetActiveDesktop()
        {
            Type typeActiveDesktop = Type.GetTypeFromCLSID(CLSID_ActiveDesktop);
            return (IActiveDesktop)Activator.CreateInstance(typeActiveDesktop);
        }

        public static string GetWallpaper()
        {
            var sb = new StringBuilder(1024);

            var ad = GetActiveDesktop();
            var ret = ad.GetWallpaper(sb, 1024, Constants.AD_GETWP_LAST_APPLIED);

            return sb.ToString();
        }

        public static void SetWallpaper(string file)
        {
            var ad = GetActiveDesktop();
            var ret = ad.SetWallpaper(file, 0);
            ad.ApplyChanges(AD_Apply.ALL);
        }


        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Save();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            SuspendPaint(textBox1);

            if (e.Control && e.KeyCode == Keys.D)
            {
                e.Handled = true;
                e.SuppressKeyPress = false;
                
                var selection = new TextSelection(textBox1);

                if (selection.IsFullLine())
                {
                    textBox1.SelectedText = "";
                }
                else
                {
                    selection.ExtendToFullLine();
                }
            }
        }

        [DllImport("user32")]
        private static extern int SendMessage(IntPtr hwnd, int wMsg, int wParam, IntPtr lParam);
        private const int WM_SETREDRAW = 0xB;

        private void SuspendPaint(Control ctrl)
        {
            SendMessage(ctrl.Handle, WM_SETREDRAW, 0, IntPtr.Zero);
        }

        //允许控件重绘.
        private void ResumePaint(Control ctrl)
        {
            SendMessage(ctrl.Handle, WM_SETREDRAW, 1, IntPtr.Zero);
        }

        /// <summary>
        /// auto change size to adapt the content of text
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            var text = textBox1.Text;

            if (text.EndsWith("\n"))
            {
                text += " ";
            }

            label1.Text = text;

            var h = label1.Height - textBox1.Height;

            if (h < 0)
            {
                h = 0;
            }

            var w = label1.Width - textBox1.Width;

            if (w < 0)
            {
                w = 0;
            }

            if (h == 0 & w == 0)
            {
                return;
            }

            this.SetBounds(this.Left, this.Top, this.Width + w, this.Height + h);
            //textBox1.Refresh();
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            ResumePaint(textBox1);
        }
    }
}
