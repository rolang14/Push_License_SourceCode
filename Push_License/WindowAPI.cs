using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace Push_License
{
    public class WindowAPI
    {
        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);

        public static void DragWindow(Window window)
        {
            WindowInteropHelper helper = new WindowInteropHelper(window);
            SendMessage(helper.Handle, 161, 2, 0);
        }

        public static void MinimizeWindow(Window window)
        {
            window.WindowState = WindowState.Minimized;
        }

        public static void CloseWindow()
        {
            Application.Current.Shutdown();
        }

        public static void setMaxHeight(Window window)
        {
            window.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
        }
    }
}
