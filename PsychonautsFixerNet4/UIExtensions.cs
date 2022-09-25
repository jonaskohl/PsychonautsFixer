using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace PsychonautsFixer
{
    public static class UIExtensions
    {
        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hwnd, uint wMsg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern uint GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, uint dwNewLong);

        private const uint BM_SETIMAGE = 0x00F7;
        private const uint BS_ICON = 0x00000040;
        private const uint IMAGE_ICON = 1;
        private const int GWL_STYLE = -16;


        public static int SetWin32Icon(this Button button, Icon icon)
        {
            if (Environment.OSVersion.Version.Major < 6)
            {
                button.Image = icon.ToBitmap();
                return 0;
            }
            return SendMessage(button.Handle, BM_SETIMAGE, new IntPtr(IMAGE_ICON), icon?.Handle ?? IntPtr.Zero);
        }
    }
}
