using Moq;
using ScreenCapture.Services;
using System.Drawing;

namespace ScreenCapture.Tests
{
    public class ScreenCaptureProgramTests
    {
        [Fact]
        public void CaptureAndCheckColor_ShouldSimulateClick_WhenColorIsPresent()
        {
            // Arrange
            var mockScreenCapture = new Mock<IScreenCaptureService>();
            var mockColorChecker = new Mock<IColorCheckerService>();
            var mockMouseSimulator = new Mock<IMouseSimulatorService>();
            var mockClickSimulator = new Mock<IClickSimulatorService>();

            Rectangle captureArea = new Rectangle(100, 100, 200, 200);
            Color color = Color.FromArgb(255, 0, 0);
            int clickOffsetX = 50;
            int clickOffsetY = 50;

            mockScreenCapture.Setup(m => m.CaptureScreen(It.IsAny<Rectangle>())).Returns(new Bitmap(1, 1));
            mockColorChecker.Setup(m => m.IsColorPresent(It.IsAny<Bitmap>(), It.IsAny<Color>())).Returns(true);

            ScreenCaptureProgram program = new ScreenCaptureProgram(mockScreenCapture.Object, mockColorChecker.Object, mockMouseSimulator.Object, mockClickSimulator.Object);

            // Act
            program.CaptureAndCheckColor(captureArea, color, clickOffsetX, clickOffsetY);

            // Assert
            mockClickSimulator.Verify(m => m.SimulateClick(captureArea.X + clickOffsetX, captureArea.Y + clickOffsetY, It.IsAny<IntPtr>()), Times.Once);
        }

        [Fact]
        public void CaptureAndCheckColor_ShouldNotSimulateClick_WhenColorIsNotPresent()
        {
            // Arrange
            var mockScreenCapture = new Mock<IScreenCaptureService>();
            var mockColorChecker = new Mock<IColorCheckerService>();
            var mockMouseSimulator = new Mock<IMouseSimulatorService>();
            var mockClickSimulator = new Mock<IClickSimulatorService>();

            Rectangle captureArea = new Rectangle(100, 100, 200, 200);
            Color color = Color.FromArgb(255, 0, 0);
            int clickOffsetX = 50;
            int clickOffsetY = 50;

            mockScreenCapture.Setup(m => m.CaptureScreen(It.IsAny<Rectangle>())).Returns(new Bitmap(1, 1));
            mockColorChecker.Setup(m => m.IsColorPresent(It.IsAny<Bitmap>(), It.IsAny<Color>())).Returns(false);

            ScreenCaptureProgram program = new ScreenCaptureProgram(mockScreenCapture.Object, mockColorChecker.Object, mockMouseSimulator.Object, mockClickSimulator.Object);

            // Act
            program.CaptureAndCheckColor(captureArea, color, clickOffsetX, clickOffsetY);

            // Assert
            mockMouseSimulator.Verify(m => m.SimulateClick(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<IntPtr>()), Times.Never);
        }

        // Additional tests for CaptureWindowAndCheckColor can be added here.
    }
}
