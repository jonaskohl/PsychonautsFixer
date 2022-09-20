using System.Reflection;
using System.Runtime.InteropServices;

namespace PsychonautsFixer
{
    internal static class Program
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr LoadCursor(IntPtr hInstance, int lpCursorName);

        [DllImport("user32.dll")]
        private static extern bool SetProcessDPIAware();

        private const int IDC_HAND = 32649;
        public static Cursor? SystemHandCursor { get; private set; }

        private static void ApplyHandCursorFix()
        {
            try
            {
                SystemHandCursor = new Cursor(LoadCursor(IntPtr.Zero, IDC_HAND));

                typeof(Cursors)?.GetField("hand", BindingFlags.Static | BindingFlags.NonPublic)?.SetValue(null, SystemHandCursor);
            }
            catch { }
        }

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            ApplyHandCursorFix();
            Application.Run(new Form1());
        }
    }
}