using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace PsychonautsFixer
{
    public static class SysIcons
    {
        const int MAX_PATH = 0x100;

        public static int LAST_ERROR { get; private set; }
        public static int LAST_WIN32_ERROR { get; private set; }

        [DllImport("Shell32.dll", SetLastError = true)]
        static extern int SHGetStockIconInfo(SHSTOCKICONID siid, SHGSI uFlags, ref SHSTOCKICONINFO psii);

        public enum SHSTOCKICONID : uint
        {
            SIID_FOLDEROPEN = 4,
            SIID_HELP = 23,
            SIID_DESKTOPPC = 94,
            SIID_USERS = 96,
        }

        [Flags]
        private enum SHGSI : uint
        {
            SHGSI_ICON = 0x000000100,
            SHGSI_LARGEICON = 0x000000000,
            SHGSI_SMALLICON = 0x000000001
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private struct SHSTOCKICONINFO
        {
            public uint cbSize;
            public IntPtr hIcon;
            public long iSysIconIndex;
            public long iIcon;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAX_PATH)]
            public string szPath;
        }

        public static Icon GetSystemIcon(SHSTOCKICONID icon, IconSize size = IconSize.Unspecified)
        {
            if (Environment.OSVersion.Version.Major < 6)
            {
                return GetSystemIconFallback(icon, size);
            }

            var info = new SHSTOCKICONINFO();
            info.cbSize = (uint)unchecked(Marshal.SizeOf(info));
            var flags = SHGSI.SHGSI_ICON;
            if (size == IconSize.Small)
                flags |= SHGSI.SHGSI_SMALLICON;
            else if (size == IconSize.Large)
                flags |= SHGSI.SHGSI_LARGEICON;

            var r = SHGetStockIconInfo(icon, flags, ref info);
            LAST_ERROR = r;
            if (r != 0)
            {
                LAST_WIN32_ERROR = Marshal.GetLastWin32Error();
                return null;
            }

            return Icon.FromHandle(info.hIcon);
        }

        private static Icon GetSystemIconFallback(SHSTOCKICONID icon, IconSize size)
        {
            switch (icon)
            {
                case SHSTOCKICONID.SIID_FOLDEROPEN:
                    return new Icon(Properties.Resources.SIID_FOLDEROPEN, GetSize(size));
                case SHSTOCKICONID.SIID_HELP:
                    return new Icon(Properties.Resources.SIID_HELP, GetSize(size));
                case SHSTOCKICONID.SIID_DESKTOPPC:
                    return new Icon(Properties.Resources.SIID_DESKTOPPC, GetSize(size));
                case SHSTOCKICONID.SIID_USERS:
                    return new Icon(Properties.Resources.SIID_USERS, GetSize(size));
                default:
                    return null;
            }
        }

        private static Size GetSize(IconSize size)
        {
            if (size == IconSize.Large)
                return new Size(32, 32);
            return new Size(16, 16);
        }

        public enum IconSize
        {
            Unspecified,
            Small,
            Large
        }
    }
}