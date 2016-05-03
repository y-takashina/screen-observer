using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScreenObserver
{
    public partial class Form1 : Form
    {
        private readonly Bitmap _bmp;
        private readonly Timer _timer;

        public Form1()
        {
            InitializeComponent();
            if (!Directory.Exists(@".\images")) Directory.CreateDirectory(@".\images");
            _bmp = new Bitmap(ClientSize.Width, ClientSize.Height);
            _timer = new Timer {Interval = 5000};
            _timer.Tick += OnTick;
        }

        public Point Center => new Point(Left + Width/2, Top + Height/2);

        private void OnTick(object o, EventArgs e)
        {
            Task.Run(async () =>
            {
                var str = Text;
                Invoke((Action) (() => { Text += @" saving"; }));
                await Task.Delay(200);
                Invoke((Action) (() => { Text += @"."; }));
                await Task.Delay(200);
                Invoke((Action) (() => { Text += @"."; }));
                await Task.Delay(200);
                Invoke((Action) (() => { Text += @"."; }));
                await Task.Delay(200);
                Invoke((Action) (() => { Text += @" saved."; }));
                await Task.Delay(500);
                Invoke((Action) (() => { Text = str; }));
            });
            var g = Graphics.FromImage(_bmp);
            g.CopyFromScreen(PointToScreen(new Point(0, 0)), new Point(0, 0), _bmp.Size);
            g.Dispose();
            _bmp.Save(@".\images\" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".png", ImageFormat.Png);
            Refresh();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Modifiers == (Keys.Control | Keys.Shift))
            {
                if (e.KeyCode == Keys.Up) Height -= 1;
                if (e.KeyCode == Keys.Down) Height += 1;
                if (e.KeyCode == Keys.Left) Width -= 1;
                if (e.KeyCode == Keys.Right) Width += 1;
            }
            else if (e.Modifiers == Keys.Control)
            {
                if (e.KeyCode == Keys.Up) Height -= 5;
                if (e.KeyCode == Keys.Down) Height += 5;
                if (e.KeyCode == Keys.Left) Width -= 5;
                if (e.KeyCode == Keys.Right) Width += 5;
            }
            else if (e.Modifiers == Keys.Shift)
            {
                if (e.KeyCode == Keys.Up) Top -= 1;
                if (e.KeyCode == Keys.Down) Top += 1;
                if (e.KeyCode == Keys.Left) Left -= 1;
                if (e.KeyCode == Keys.Right) Left += 1;
            }
            else
            {
                if (e.KeyCode == Keys.Up) Top -= 5;
                if (e.KeyCode == Keys.Down) Top += 5;
                if (e.KeyCode == Keys.Left) Left -= 5;
                if (e.KeyCode == Keys.Right) Left += 5;
            }


            var g = Graphics.FromImage(_bmp);
            g.CopyFromScreen(PointToScreen(new Point(0, 0)), new Point(0, 0), _bmp.Size);
            if (e.KeyCode == Keys.Space || e.KeyCode == Keys.Enter)
            {
                if (!_timer.Enabled)
                {
                    _timer.Start();
                    Text = @"Screen Observer - 録画中";
                }
                else
                {
                    _timer.Stop();
                    Text = @"Screen Observer - 停止";
                }
            }
            g.Dispose();
            Refresh();
        }
    }
}