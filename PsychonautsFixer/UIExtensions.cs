using System.Runtime.InteropServices;

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

        public static int SetWin32Icon(this Button button, Icon? icon)
        {
            //var style = GetWindowLong(button.Handle, GWL_STYLE);
            //style |= BS_ICON;
            //SetWindowLong(button.Handle, GWL_STYLE, style);
            return SendMessage(button.Handle, BM_SETIMAGE, new IntPtr(IMAGE_ICON), icon?.Handle ?? IntPtr.Zero);
        }
    }
}
