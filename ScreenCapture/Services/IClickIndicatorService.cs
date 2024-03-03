using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenCapture.Services
{
    public interface IClickIndicatorService
    {
        void ShowClickIndicator(Point location, int size, Color color);
    }
}
