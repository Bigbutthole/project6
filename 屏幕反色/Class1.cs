using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

namespace 屏幕反色
{
    public class ApiClass
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern int GetShortPathName(
string lpszLongPath,
string shortFile,
int cchBuffer
);

        [DllImport("winmm.dll", EntryPoint = "mciSendString", CharSet = CharSet.Auto)]
        public static extern int mciSendString(
        string lpstrCommand,
        string lpstrReturnString,
        int uReturnLength,
        int hwndCallback
        );
        [DllImport("User32.dll")]
        public static extern IntPtr GetWindowDC(IntPtr hwnd);

        [DllImport("user32", EntryPoint = "GetDesktopWindow")]
        public static extern IntPtr GetDesktopWindow();
        [DllImport("gdi32.dll")]
        public static extern bool BitBlt(IntPtr hdcDest, int nXDest, int nYDest, int wDest, int hDest, IntPtr hdcSource, int xSrc, int ySrc, CopyPixelOperation rop);
        [DllImport("kernel32.dll")]
        public static extern bool Beep(int freq, int duration);
        [DllImport("winmm.dll")]
        public static extern long waveOutSetVolume(UInt32 deviceID, Int32 Volumea);
        [DllImport("winmm.dll")]
        public static extern long waveOutGetVolume(UInt32 deviceID, out Int32 Volumea);
        //寻找目标进程窗口
        [DllImport("USER32.DLL")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("USER32.DLL", EntryPoint = "FindWindowEx", SetLastError = true)]
        public static extern IntPtr FindWindowEx(IntPtr hwndParent, uint hwndChildAfter, string lpszClass, string lpszWindow);
        //设置进程窗口到最前
        [DllImport("USER32.DLL")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);
        //模拟键盘事件
        [DllImport("USER32.DLL")]
        public static extern void keybd_event(Byte bVk, Byte bScan, Int32 dwFlags, Int32 dwExtraInfo);
        public delegate bool CallBack(IntPtr hwnd, int lParam);
        [DllImport("USER32.DLL")]
        public static extern int EnumChildWindows(IntPtr hWndParent, CallBack lpfn, int lParam);
        //给CheckBox发送信息
        [DllImport("USER32.DLL", EntryPoint = "SendMessage", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int SendMessage(IntPtr hwnd, UInt32 wMsg, int wParam, int lParam);
        //给Text发送信息
        [DllImport("USER32.DLL", EntryPoint = "SendMessage")]
        private static extern int SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, string lParam);
        [DllImport("USER32.DLL")]
        public static extern IntPtr GetWindow(IntPtr hWnd, int wCmd);
        [DllImport("USER32.DLL")]
        public static extern bool SwitchDesktop(IntPtr hDesktop);
    }
    public class Class1
    {
        public static Bitmap PContray(Bitmap a)
        {
            int w = a.Width;
            int h = a.Height;
            Bitmap dstBitmap = new Bitmap(a.Width, a.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb); //创造一个临时值
            System.Drawing.Imaging.BitmapData srcData = a.LockBits(new Rectangle(0, 0, w, h), System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb); //锁定位图
            System.Drawing.Imaging.BitmapData dstData = dstBitmap.LockBits(new Rectangle(0, 0, w, h), System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb); //锁定位图
            unsafe //执行不安全代码
            {
                byte* pIn = (byte*)srcData.Scan0.ToPointer(); //获得指向byte数组的指针
                byte* pOut = (byte*)dstData.Scan0.ToPointer(); //同上
                byte* p; //临时变量
                int stride = srcData.Stride; //获得a的扫描行数（和y值差不多但是不是y值）
                int r, g, b; //R！G！B！信仰色
                for (int y = 0; y < h; y++) //每个扫描行处理一遍
                {
                    for (int x = 0; x < w; x++) //每个扫描行的点处理一边
                    {
                        p = pIn; //复制pIn到p
                        r = p[2]; //获得目前点的R！值
                        g = p[1]; //获得目前点的G！值
                        b = p[0]; //获得目前点的B！值
                        pOut[2] = (byte)(255 - r); //反色=值的类型的最大值-值
                        pOut[1] = (byte)(255 - g);
                        pOut[0] = (byte)(255 - b);
                        pIn += 3; //移动到另一个同行的像素点
                        pOut += 3; //同上
                    }
                    pIn += srcData.Stride - w * 3; //移动到另外一行
                    pOut += srcData.Stride - w * 3; //同上
                }
                a.UnlockBits(srcData); //将更改同步到a
                dstBitmap.UnlockBits(dstData); //同上
                return dstBitmap; //返回dstBitmap
            }
        }
    }
}
