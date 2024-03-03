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
        public static extern bool ScreenToClient(IntPtr hWnd, ref Point lpPoint);

        [DllImport("user32.dll")]
        public static extern IntPtr ChildWindowFromPoint(IntPtr hWndParent, Point Point);

        [DllImport("user32.dll")]
        public static extern bool ClientToScreen(IntPtr hWnd, ref Point lpPoint);


        const int SM_CYCAPTION = 4;
        const uint WM_LBUTTONDOWN = 0x0201;
        const uint WM_LBUTTONUP = 0x0202;
        const uint MK_LBUTTON = 0x0001;

        private readonly IClickIndicatorService _clickIndicatorService;

        public ClickSimulatorService(IClickIndicatorService clickIndicatorService)
        {
            _clickIndicatorService = clickIndicatorService;
        }

        public static void SimulateClickStatic(int x, int y, IntPtr windowHandle)
        {
            if (windowHandle == IntPtr.Zero)
            {
                throw new ArgumentException("Window handle cannot be IntPtr.Zero.", nameof(windowHandle));
            }

            Console.WriteLine($"Main window handle: {windowHandle:x}");

            // Adjust the y-coordinate for the height of the title bar
            int titleBarHeight = GetSystemMetrics(SM_CYCAPTION);
            Console.WriteLine($"Title bar height: {titleBarHeight}");

            y -= titleBarHeight;
            Console.WriteLine($"Adjusted coordinates for click: ({x}, {y})");

            // Get the window handle at the specified location
            IntPtr handleAtLocation = ChildWindowFromPoint(windowHandle, new Point(x, y));
            Console.WriteLine($"Handle at location ({x}, {y}): {handleAtLocation:x}");

            // Convert the main window coordinates to screen coordinates
            Point screenPoint = new Point(x, y);
            ClientToScreen(windowHandle, ref screenPoint);

            // Convert the screen coordinates to the client-area coordinates of the child control
            Point childControlPoint = new Point(screenPoint.X, screenPoint.Y);
            ScreenToClient(handleAtLocation, ref childControlPoint);

            if (handleAtLocation != windowHandle)
            {
                Console.WriteLine($"Click will be sent to a child window with handle {handleAtLocation:x} X:{screenPoint.X} Y:{screenPoint.Y}");
            }
            else
            {
                Console.WriteLine($"Click will be sent to the main window");
            }

            // Convert the coordinates to the LPARAM format
            IntPtr lParam = (IntPtr)((childControlPoint.Y << 16) | childControlPoint.X);

            // Send the mouse down and up messages to simulate a click
            Console.WriteLine($"Sending WM_LBUTTONDOWN to handle {handleAtLocation:x} X:{childControlPoint.X} Y:{childControlPoint.Y}");
            SendMessage(handleAtLocation, WM_LBUTTONDOWN, (IntPtr)MK_LBUTTON, lParam);

            Console.WriteLine($"Sending WM_LBUTTONUP to handle {handleAtLocation:x} X:{childControlPoint.X} Y:{childControlPoint.Y}");
            SendMessage(handleAtLocation, WM_LBUTTONUP, IntPtr.Zero, lParam);
        }



        public void SimulateClick(int x, int y, IntPtr windowHandle)
        {
            // Instance method simply calls the static method
            SimulateClickStatic(x, y, windowHandle);
            _clickIndicatorService.ShowClickIndicator(new Point(x, y), 20, Color.Green);
        }
    }
}
