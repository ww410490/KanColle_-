using System.Runtime.InteropServices;

namespace ex9_控制滑鼠移動點擊_自動遠征_
{
    using System;
    using System.Windows.Forms;
    using System.Runtime.InteropServices;
    using static Mouse;
    using System.Configuration;

    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();

            // 每隔一段时间执行一次显示鼠标位置的操作
            Timer timer = new Timer();
            timer.Interval = 100;
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }


        private void timer_Tick(object sender, EventArgs e)
        {
            // 获取鼠标当前位置并将其显示在文本框中
            Point mousePos = Cursor.Position;
            textBox1.Text = "X:" + mousePos.X + " Y:" + mousePos.Y;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Point mousePos = Cursor.Position;
            textBox2.Text += "X:" + mousePos.X + " Y:" + mousePos.Y + Environment.NewLine;
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            // 移动鼠标到指定位置
            //x：矩形左上角的 x 坐标；y：矩形左上角的 y 坐标；width：矩形的宽度；height：矩形的高度。
            //先在首頁收遠征
            //string rectangle1Value = ConfigurationManager.AppSettings["Rectangle1"]; // 设置矩形区域  
            //MoveTo(rectangle1Value);
            //LeftClick();

            foreach (string key in ConfigurationManager.AppSettings.AllKeys)
            {
                string value = ConfigurationManager.AppSettings[key];
                textBox2.Text += value + Environment.NewLine;
                MoveToAsync(value);

                Random random = new Random();
                int sec1 = random.Next(1000, 3000);
                await Task.Delay(sec1);

                LeftClick();
            }

        }

        static public async Task MoveToAsync(string rectangle1Value)
        {
            //string rectangle1Value = ConfigurationManager.AppSettings["Rectangle1"];
            string[] rectangle1Values = rectangle1Value.Split(',');
            int rectx = int.Parse(rectangle1Values[0]);
            int recty = int.Parse(rectangle1Values[1]);
            int width = int.Parse(rectangle1Values[2]);
            int height = int.Parse(rectangle1Values[3]);
            Rectangle rect = new Rectangle(rectx, recty, width, height);

            Random random = new Random();
            int x = random.Next(rect.Left, rect.Right);
            int y = random.Next(rect.Top, rect.Bottom);
            Cursor.Position = new Point(x, y);

            int sec1 = random.Next(1000, 2000);
            await Task.Delay(sec1);
        }

        static public async void LeftClick()
        {
            Random random = new Random();
            int sec1 = random.Next(500, 1000);
            int sec2 = random.Next(10, 20);
            await Task.Delay(sec1);
            LeftDown();
            await Task.Delay(sec2);
            LeftUp();
        }

        static public void LeftDown()
        {
            INPUT leftdown = new INPUT();

            leftdown.dwType = 0;
            leftdown.mi = new MOUSEINPUT();
            leftdown.mi.dwExtraInfo = IntPtr.Zero;
            leftdown.mi.dx = 0;
            leftdown.mi.dy = 0;
            leftdown.mi.time = 0;
            leftdown.mi.mouseData = 0;
            leftdown.mi.dwFlags = MOUSEFLAG.LEFTDOWN;

            SendInput(1, ref leftdown, Marshal.SizeOf(typeof(INPUT)));
        }

        static public void LeftUp()
        {
            INPUT leftup = new INPUT();

            leftup.dwType = 0;
            leftup.mi = new MOUSEINPUT();
            leftup.mi.dwExtraInfo = IntPtr.Zero;
            leftup.mi.dx = 0;
            leftup.mi.dy = 0;
            leftup.mi.time = 0;
            leftup.mi.mouseData = 0;
            leftup.mi.dwFlags = MOUSEFLAG.LEFTUP;

            SendInput(1, ref leftup, Marshal.SizeOf(typeof(INPUT)));
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox2.Text = "";
        }
    }    
}

static public class Mouse
{
    [DllImport("user32.dll", SetLastError = true)]
    public static extern Int32 SendInput(Int32 cInputs, ref INPUT pInputs, Int32 cbSize);

    [StructLayout(LayoutKind.Explicit, Pack = 1, Size = 28)]
    public struct INPUT
    {
        [FieldOffset(0)]
        public INPUTTYPE dwType;
        [FieldOffset(4)]
        public MOUSEINPUT mi;
        [FieldOffset(4)]
        public KEYBOARDINPUT ki;
        [FieldOffset(4)]
        public HARDWAREINPUT hi;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MOUSEINPUT
    {
        public Int32 dx;
        public Int32 dy;
        public Int32 mouseData;
        public MOUSEFLAG dwFlags;
        public Int32 time;
        public IntPtr dwExtraInfo;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct KEYBOARDINPUT
    {
        public Int16 wVk;
        public Int16 wScan;
        public KEYBOARDFLAG dwFlags;
        public Int32 time;
        public IntPtr dwExtraInfo;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct HARDWAREINPUT
    {
        public Int32 uMsg;
        public Int16 wParamL;
        public Int16 wParamH;
    }

    public enum INPUTTYPE : int
    {
        Mouse = 0,
        Keyboard = 1,
        Hardware = 2
    }

    [Flags()]
    public enum MOUSEFLAG : int
    {
        MOVE = 0x1,
        LEFTDOWN = 0x2,
        LEFTUP = 0x4,
        RIGHTDOWN = 0x8,
        RIGHTUP = 0x10,
        MIDDLEDOWN = 0x20,
        MIDDLEUP = 0x40,
        XDOWN = 0x80,
        XUP = 0x100,
        VIRTUALDESK = 0x400,
        WHEEL = 0x800,
        ABSOLUTE = 0x8000
    }

    [Flags()]
    public enum KEYBOARDFLAG : int
    {
        EXTENDEDKEY = 1,
        KEYUP = 2,
        UNICODE = 4,
        SCANCODE = 8
    }
}