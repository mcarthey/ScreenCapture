using System.Runtime.InteropServices;

public class GdiDrawingService : IDrawingService
{
    [DllImport("user32.dll")]
    private static extern IntPtr GetDC(IntPtr hWnd);

    [DllImport("user32.dll")]
    private static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

    [DllImport("gdi32.dll")]
    private static extern IntPtr CreatePen(int fnPenStyle, int nWidth, uint crColor);

    [DllImport("gdi32.dll")]
    private static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);

    [DllImport("gdi32.dll")]
    private static extern bool MoveToEx(IntPtr hDC, int X, int Y, IntPtr lpPoint);

    [DllImport("gdi32.dll")]
    private static extern bool LineTo(IntPtr hDC, int nXEnd, int nYEnd);

    [DllImport("gdi32.dll")]
    private static extern bool DeleteObject(IntPtr hObject);

    public void DrawX(IntPtr hWnd, int x, int y, int size, int penWidth, uint color)
    {
        IntPtr hDC = GetDC(hWnd);
        IntPtr hPen = CreatePen(0, penWidth, color);
        IntPtr hOldPen = SelectObject(hDC, hPen);

        MoveToEx(hDC, x - size, y - size, IntPtr.Zero);
        LineTo(hDC, x + size, y + size);
        MoveToEx(hDC, x + size, y - size, IntPtr.Zero);
        LineTo(hDC, x - size, y + size);

        SelectObject(hDC, hOldPen);
        DeleteObject(hPen);
        ReleaseDC(hWnd, hDC);
    }
}