using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScreenCapture.Forms
{
    public class ClickOverlayForm : Form
    {
        public ClickOverlayForm(Point location, int size, Color color)
        {
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(location.X - size / 2, location.Y - size / 2);
            this.Size = new Size(size, size);
            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = Color.Magenta; // Use a color that can be made transparent
            this.TransparencyKey = this.BackColor; // Make the form transparent except for the drawing
            this.TopMost = true; // Ensure the overlay is always on top

            // Draw the "X" indicator
            Paint += (sender, e) =>
            {
                using (Pen pen = new Pen(color, 2))
                {
                    e.Graphics.DrawLine(pen, 0, 0, size, size);
                    e.Graphics.DrawLine(pen, size, 0, 0, size);
                }
            };
        }

        protected override bool ShowWithoutActivation => true; // Prevent the overlay from taking focus
    }

}
