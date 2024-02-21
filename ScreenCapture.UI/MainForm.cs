namespace ScreenCapture.UI
{
    public class MainForm : Form
    {
        private Button btnTest;
        private PictureBox pictureBox;
        private Label lblClickCount;
        private int clickCount = 0;
        private int redBoxClickCount = 0;

        private System.ComponentModel.IContainer components = null;

        public MainForm()
        {
            InitializeComponent();
            this.Text = "My Test Window"; // Set the window title
        }

        private void InitializeComponent()
        {
            this.btnTest = new Button();
            this.pictureBox = new PictureBox();
            this.lblClickCount = new Label();

            // Button
            this.btnTest.Location = new Point(10, 10);
            this.btnTest.Size = new Size(100, 30);
            this.btnTest.Text = "Click Me";
            this.btnTest.Click += new EventHandler(this.btnTest_Click);

            // PictureBox
            this.pictureBox.Location = new Point(10, 50);
            this.pictureBox.Size = new Size(100, 100);
            this.pictureBox.BackColor = Color.White;
            this.pictureBox.Paint += new PaintEventHandler(this.pictureBox_Paint);
            this.pictureBox.Click += new EventHandler(this.pictureBox_Click);

            // Label
            this.lblClickCount.Location = new Point(10, 160);
            this.lblClickCount.Size = new Size(100, 30);
            this.lblClickCount.Text = "Clicks: 0";

            // Form
            this.ClientSize = new Size(200, 200);
            this.Controls.Add(this.btnTest);
            this.Controls.Add(this.pictureBox);
            this.Controls.Add(this.lblClickCount);
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            clickCount++;
            lblClickCount.Text = $"Clicks: {clickCount}";
        }

        private void pictureBox_Paint(object sender, PaintEventArgs e)
        {
            // Draw a 20x20 red box in the center of the PictureBox
            int boxSize = 20;
            int centerX = (pictureBox.Width - boxSize) / 2;
            int centerY = (pictureBox.Height - boxSize) / 2;
            e.Graphics.FillRectangle(Brushes.Red, centerX, centerY, boxSize, boxSize);

            // Draw the click counter in the top-left corner of the red box
            if (redBoxClickCount > 0)
            {
                e.Graphics.DrawString(redBoxClickCount.ToString(), this.Font, Brushes.Black, centerX, centerY);
            }
        }

        private void pictureBox_Click(object sender, EventArgs e)
        {
            // Convert the click location to be relative to the PictureBox
            Point clickLocation = pictureBox.PointToClient(Cursor.Position);

            // Check if the click is within the red box
            int boxSize = 20;
            int centerX = (pictureBox.Width - boxSize) / 2;
            int centerY = (pictureBox.Height - boxSize) / 2;
            Rectangle redBoxBounds = new Rectangle(centerX, centerY, boxSize, boxSize);

            if (redBoxBounds.Contains(clickLocation))
            {
                redBoxClickCount++;
                pictureBox.Invalidate(); // Trigger a repaint to update the counter
            }
        }

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
