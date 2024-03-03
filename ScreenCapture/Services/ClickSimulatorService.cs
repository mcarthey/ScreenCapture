using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ScreenCapture.Services
{
    public class ClickSimulatorService : IClickSimulatorService
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern int GetSystemMetrics(int nIndex);

        [DllImport("user32.dll")]
        public static extern IntPtr WindowFromPoint(Point point);

        [DllImport("user32.dll")]
        public static extern IntPtr ChildWindowFromPoint(IntPtr hWndParent, Point Point);



        const int SM_CYCAPTION = 4;
        const uint WM_LBUTTONDOWN = 0x0201;
        const uint WM_LBUTTONUP = 0x0202;
        const uint MK_LBUTTON = 0x0001;

        public static void SimulateClickStatic(int x, int y, IntPtr windowHandle)
        {
            if (windowHandle == IntPtr.Zero)
            {
                throw new ArgumentException("Window handle cannot be IntPtr.Zero.", nameof(windowHandle));
            }

            // Adjust the y-coordinate for the height of the title bar
            int titleBarHeight = GetSystemMetrics(SM_CYCAPTION);
            y -= titleBarHeight;

            // Get the window handle at the specified location
            IntPtr handleAtLocation = ChildWindowFromPoint(windowHandle, new Point(x, y));

            // Convert the coordinates to the LPARAM format
            IntPtr lParam = (IntPtr)((y << 16) | x);

            // Send the mouse down and up messages to simulate a click
            SendMessage(handleAtLocation, WM_LBUTTONDOWN, (IntPtr)MK_LBUTTON, lParam);
            SendMessage(handleAtLocation, WM_LBUTTONUP, IntPtr.Zero, lParam);

            // Output to the console the details of the click
            Console.WriteLine($"Simulated a click at ({x}, {y}) on window with handle {windowHandle}");
        }

        public void SimulateClick(int x, int y, IntPtr windowHandle)
        {
            // Instance method simply calls the static method
            SimulateClickStatic(x, y, windowHandle);
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
        public int X;
        public int Y;

        public POINT(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
