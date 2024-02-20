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

        public ScreenCaptureProgram(IScreenCaptureService screenCaptureService, IColorCheckerService colorCheckerService, IMouseSimulatorService mouseSimulatorService)
        {
            _screenCaptureService = screenCaptureService;
            _colorCheckerService = colorCheckerService;
            _mouseSimulatorService = mouseSimulatorService;
        }


        public static void Main(string[] args)
        {
            ScreenCaptureServiceService screenCapture = new ScreenCaptureServiceService();
            ColorCheckerService colorCheckerService = new ColorCheckerService();
            MouseSimulatorService mouseSimulatorService = new MouseSimulatorService();

            ScreenCaptureProgram program = new ScreenCaptureProgram(screenCapture, colorCheckerService, mouseSimulatorService);

            Rectangle captureArea = new Rectangle(100, 100, 200, 200);
            program.CaptureAndCheckColor(captureArea, Color.FromArgb(255, 0, 0), 50, 50);

            program.CaptureWindowAndCheckColor("Untitled - Paint", Color.FromArgb(255, 0, 0), 50, 50);
         
            Bitmap targetImage = new Bitmap("path/to/your/target/image.bmp");
            program.CaptureWindowAndClickImage("Your Window Title Here", targetImage, 10, 10);
        }

        public void CaptureAndCheckColor(Rectangle captureArea, Color color, int clickOffsetX, int clickOffsetY)
        {
            using (Bitmap capturedScreen = _screenCaptureService.CaptureScreen(captureArea))
            {
                if (_colorCheckerService.IsColorPresent(capturedScreen, color))
                {
                    _mouseSimulatorService.SimulateClick(captureArea.X + clickOffsetX, captureArea.Y + clickOffsetY);
                }
            }
        }

        public void CaptureWindowAndCheckColor(string windowTitle, Color color, int clickX, int clickY)
        {
            using (Bitmap bitmap = _screenCaptureService.CaptureWindow(windowTitle))
            {
                if (_colorCheckerService.IsColorPresent(bitmap, color))
                {
                    _mouseSimulatorService.SimulateClick(clickX, clickY);
                }
            }
        }

        public void CaptureWindowAndClickImage(string windowTitle, Bitmap targetImage, int clickOffsetX, int clickOffsetY)
        {
            using (Bitmap bitmap = _screenCaptureService.CaptureWindow(windowTitle))
            {
                Point? location = ImageMatcherService.FindTargetInSource(bitmap, targetImage);
                if (location.HasValue)
                {
                    _mouseSimulatorService.SimulateClick(location.Value.X + clickOffsetX, location.Value.Y + clickOffsetY);
                }
            }
        }


    }
}
