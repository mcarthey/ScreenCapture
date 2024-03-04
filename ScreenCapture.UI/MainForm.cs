namespace ScreenCapture.UI
{
    public class MainForm : Form
    {
        private Button btnTest;
        private PictureBox pictureBox;
        private Label lblClickCount;
        private Label lblMousePosition;
        private Label lblPictureBoxMousePosition;
        private TextBox txtLog;
        private int clickCount = 0;
        private int redBoxClickCount = 0;

        private System.ComponentModel.IContainer components = null;

        public MainForm()
        {
            InitializeComponent();
            this.Text = "My Test Window"; // Set the window title
            this.MouseMove += MainForm_MouseMove;
        }

        private void InitializeComponent()
        {
            btnTest = new Button();
            pictureBox = new PictureBox();
            lblClickCount = new Label();
            lblMousePosition = new Label();
            lblPictureBoxMousePosition = new Label();
            txtLog = new TextBox();
            ((System.ComponentModel.ISupportInitialize)pictureBox).BeginInit();
            SuspendLayout();
            // 
            // btnTest
            // 
            btnTest.Location = new Point(10, 10);
            btnTest.Name = "btnTest";
            btnTest.Size = new Size(100, 30);
            btnTest.TabIndex = 1;
            btnTest.Text = "Click Me";
            btnTest.Click += btnTest_Click;
            // 
            // pictureBox
            // 
            pictureBox.BackColor = Color.White;
            pictureBox.Location = new Point(10, 50);
            pictureBox.Name = "pictureBox";
            pictureBox.Size = new Size(100, 100);
            pictureBox.TabIndex = 2;
            pictureBox.TabStop = false;
            pictureBox.Click += pictureBox_Click;
            pictureBox.Paint += pictureBox_Paint;
            pictureBox.MouseMove += PbForm_MouseMove;
            // 
            // lblClickCount
            // 
            lblClickCount.Location = new Point(10, 160);
            lblClickCount.Name = "lblClickCount";
            lblClickCount.Size = new Size(100, 20);
            lblClickCount.TabIndex = 3;
            lblClickCount.Text = "Clicks: 0";
            // 
            // lblMousePosition
            // 
            lblMousePosition.Location = new Point(10, 180);
            lblMousePosition.Name = "lblMousePosition";
            lblMousePosition.Size = new Size(100, 20);
            lblMousePosition.TabIndex = 4;
            lblMousePosition.Text = "Coord: 0, 0";
            // 
            // lblPictureBoxMousePosition
            // 
            lblPictureBoxMousePosition.Location = new Point(10, 200);
            lblPictureBoxMousePosition.Name = "lblPictureBoxMousePosition";
            lblPictureBoxMousePosition.Size = new Size(100, 20);
            lblPictureBoxMousePosition.TabIndex = 5;
            lblPictureBoxMousePosition.Text = "PB Coord: 0, 0";
            // 
            // txtLog
            // 
            txtLog.Location = new Point(10, 220);
            txtLog.Multiline = true;
            txtLog.Name = "txtLog";
            txtLog.Size = new Size(180, 100);
            txtLog.TabIndex = 0;
            // 
            // MainForm
            // 
            ClientSize = new Size(200, 332);
            Controls.Add(btnTest);
            Controls.Add(pictureBox);
            Controls.Add(lblClickCount);
            Controls.Add(lblMousePosition);
            Controls.Add(lblPictureBoxMousePosition);
            Controls.Add(txtLog);
            Name = "MainForm";
            ((System.ComponentModel.ISupportInitialize)pictureBox).EndInit();
            ResumeLayout(false);
            PerformLayout();
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

        private void MainForm_MouseMove(object sender, MouseEventArgs e)
        {
            // Display the coordinates in the lblMousePosition label
            lblMousePosition.Text = $"Coord: {e.X}, {e.Y}";
        }

        private void PbForm_MouseMove(object sender, MouseEventArgs e)
        {
            // Convert the mouse position to be relative to the PictureBox
            Point pictureBoxCoord = pictureBox.PointToClient(Cursor.Position);

            // Add another label to display the PictureBox coordinates
            lblPictureBoxMousePosition.Text = $"PB Coord: {pictureBoxCoord.X}, {pictureBoxCoord.Y}";
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

        protected override void WndProc(ref Message m)
        {
            txtLog.AppendText($"Received message: {m.Msg}\r\n");

            base.WndProc(ref m);
        }
    }
}
