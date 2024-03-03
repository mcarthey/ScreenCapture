using System.Drawing;
using ScreenCapture.Forms;

namespace ScreenCapture.Services;

public class ClickOverlayIndicatorService : IClickIndicatorService
{
    public void ShowClickIndicator(Point location, int size, Color color)
    {
        ClickOverlayForm overlay = new ClickOverlayForm(location, size, color);
        overlay.Show();

        // Use the Invoke method to ensure that the Close method is executed on the UI thread
        Task.Delay(500).ContinueWith(_ =>
        {
            if (overlay.InvokeRequired)
            {
                overlay.Invoke(new MethodInvoker(overlay.Close));
            }
            else
            {
                overlay.Close();
            }
        });
    }
}
