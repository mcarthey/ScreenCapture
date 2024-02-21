using System.Drawing;
using System;
using ScreenCapture.Services;

namespace ScreenCapture
{
    public class ScreenCaptureProgram
    {
        private readonly IScreenCaptureService _screenCaptureService;
        private readonly IColorCheckerService _colorCheckerService;
        private readonly IMouseSimulatorService _mouseSimulatorService;
        private IntPtr _windowHandle;

        public ScreenCaptureProgram(IScreenCaptureService screenCaptureService, IColorCheckerService colorCheckerService, IMouseSimulatorService mouseSimulatorService)
        {
            _screenCaptureService = screenCaptureService;
            _colorCheckerService = colorCheckerService;
            _mouseSimulatorService = mouseSimulatorService;
        }


        public static void Main(string[] args)
        {
            ScreenCaptureService screenCapture = new ScreenCaptureService();
            ColorCheckerService colorCheckerService = new ColorCheckerService();
            MouseSimulatorService mouseSimulatorService = new MouseSimulatorService();

            ScreenCaptureProgram program = new ScreenCaptureProgram(screenCapture, colorCheckerService, mouseSimulatorService);

            //Rectangle captureArea = new Rectangle(100, 100, 200, 200);
            //program.CaptureAndCheckColor(captureArea, Color.FromArgb(255, 0, 0), 50, 50);
            //program.CaptureWindowAndCheckColor("Untitled - Paint", Color.FromArgb(255, 0, 0), 50, 50);
         
            Bitmap targetImage = new Bitmap("Images/target.bmp");

            while (true)
            {
                Console.WriteLine("Press Enter to capture and click image, or any other key to exit.");
                var key = Console.ReadKey();
                if (key.Key == ConsoleKey.Enter)
                {
                    program.CaptureWindowAndClickImage("My Test Window", targetImage, 10, 10);
                }
                else
                {
                    break;
                }
            }
        }

        public void CaptureAndCheckColor(Rectangle captureArea, Color color, int clickOffsetX, int clickOffsetY)
        {
            using (Bitmap capturedScreen = _screenCaptureService.CaptureScreen(captureArea))
            {
                if (_colorCheckerService.IsColorPresent(capturedScreen, color))
                {
                    _mouseSimulatorService.SimulateClick(captureArea.X + clickOffsetX, captureArea.Y + clickOffsetY, IntPtr.Zero);
                }
            }
        }

        public void CaptureWindowAndCheckColor(string windowTitle, Color color, int clickX, int clickY)
        {
            using (Bitmap bitmap = _screenCaptureService.CaptureWindow(windowTitle, out _windowHandle))
            {
                if (_colorCheckerService.IsColorPresent(bitmap, color))
                {
                    _mouseSimulatorService.SimulateClick(clickX, clickY, _windowHandle);
                }
            }
        }

        public void CaptureWindowAndClickImage(string windowTitle, Bitmap targetImage, int clickOffsetX, int clickOffsetY)
        {
            using (Bitmap bitmap = _screenCaptureService.CaptureWindow(windowTitle, out _windowHandle))
            {
                bitmap.Save("capturedWindow.png");

                Point? location = ImageMatcherService.FindTargetInSource(bitmap, targetImage);
                if (location.HasValue)
                {
                    _mouseSimulatorService.SimulateClick(location.Value.X + clickOffsetX, location.Value.Y + clickOffsetY, _windowHandle);
                }
            }
        }


    }
}
