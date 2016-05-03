using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading.Tasks;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace ScreenObserver
{
    public partial class Form1 : Form
    {
        private readonly Bitmap _bmp;
        private readonly Timer _timer;

        public Point Center => new Point(Left + Width/2, Top + Height/2);

        public Form1()
        {
            InitializeComponent();
            _bmp = new Bitmap(ClientSize.Width, ClientSize.Height);
            _timer = new Timer {Interval = 5000};
            _timer.Tick += OnTick;
        }

        private void OnTick(object o, EventArgs e)
        {
            Task.Run(async () =>
            {
                string str = Text;
                Invoke((Action)(() => { Text += @" saving";}));
                await Task.Delay(200);
                Invoke((Action)(() => { Text += @"."; }));
                await Task.Delay(200);
                Invoke((Action)(() => { Text += @"."; }));
                await Task.Delay(200);
                Invoke((Action)(() => { Text += @"."; }));
                await Task.Delay(200);
                Invoke((Action)(() => { Text += @" saved."; }));
                await Task.Delay(500);
                Invoke((Action)(() => { Text = str; }));
            });
            Graphics g = Graphics.FromImage(_bmp);
            g.CopyFromScreen(PointToScreen(new Point(0, 0)), new Point(0, 0), _bmp.Size);
            g.Dispose();
            _bmp.Save(@".\img\" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".png", ImageFormat.Png);
            Refresh();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up)    Top -= 1;
            if (e.KeyCode == Keys.Down)  Top += 1;
            if (e.KeyCode == Keys.Left)  Left -= 1;
            if (e.KeyCode == Keys.Right) Left += 1;

            Graphics g = Graphics.FromImage(_bmp);
            g.CopyFromScreen(PointToScreen(new Point(0, 0)), new Point(0, 0), _bmp.Size);
            if (e.KeyCode == Keys.Back) g.Clear(BackColor);
            if (e.KeyCode == Keys.Space)
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
