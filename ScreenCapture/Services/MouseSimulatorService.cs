using WindowsInput;

namespace ScreenCapture.Services;

public class MouseSimulatorService : IMouseSimulatorService
{
    public void SimulateClick(int x, int y)
    {
        var inputSimulator = new InputSimulator();
        inputSimulator.Mouse.MoveMouseToPositionOnVirtualDesktop(x, y);
        inputSimulator.Mouse.LeftButtonClick();
    }
}
