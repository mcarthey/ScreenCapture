namespace ScreenCapture.Services;

public interface IClickSimulatorService
{
    void SimulateClick(int x, int y, IntPtr windowHandle);
}
