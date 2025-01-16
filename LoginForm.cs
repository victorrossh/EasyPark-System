using MySql.Data.MySqlClient;
using park_control.Database;
using System.Runtime.InteropServices;

namespace park_control
{
    public partial class LoginForm : Form
    {
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HT_CAPTION = 0x2;

        public LoginForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            // Form settings
            this.Text = "Login - Parking System";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.None;
            this.MinimumSize = new Size(720, 540);
            this.MaximumSize = new Size(1024, 768);
            this.Size = new Size(720, 540);
            this.BackColor = Color.FromArgb(28, 28, 30);
            this.Padding = new Padding(2);

            // Custom title bar
            Panel titleBar = new Panel
            {
                BackColor = Color.FromArgb(38, 38, 40),
                Dock = DockStyle.Top,
                Height = 32
            };

            Label titleLabel = new Label
            {
                Text = "Login - Parking System",
                ForeColor = Color.FromArgb(225, 225, 225),
                Font = new Font("Segoe UI", 9F),
                Location = new Point(10, 8),
                AutoSize = true
            };

            // Buttons
            Button minimizeButton = new Button
            {
                Text = "─",
                Size = new Size(32, 32),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.Transparent,
                ForeColor = Color.FromArgb(225, 225, 225),
                Font = new Font("Segoe UI", 9F),
                Cursor = Cursors.Hand,
                Dock = DockStyle.Right,
                FlatAppearance = { BorderSize = 0 }
            };

            Button maximizeButton = new Button
            {
                Text = "□",
                Size = new Size(32, 32),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.Transparent,
                ForeColor = Color.FromArgb(225, 225, 225),
                Font = new Font("Segoe UI", 9F),
                Cursor = Cursors.Hand,
                Dock = DockStyle.Right,
                FlatAppearance = { BorderSize = 0 }
            };

            Button closeButton = new Button
            {
                Text = "✕",
                Size = new Size(32, 32),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.Transparent,
                ForeColor = Color.FromArgb(225, 225, 225),
                Font = new Font("Segoe UI", 9F),
                Cursor = Cursors.Hand,
                Dock = DockStyle.Right,
                FlatAppearance = { BorderSize = 0 }
            };

           // Button functions
            closeButton.Click += (s, e) => this.Close();
            closeButton.MouseEnter += (s, e) => closeButton.BackColor = Color.FromArgb(232, 17, 35);
            closeButton.MouseLeave += (s, e) => closeButton.BackColor = Color.Transparent;

            maximizeButton.MouseEnter += (s, e) => maximizeButton.BackColor = Color.FromArgb(50, 50, 52);
            maximizeButton.MouseLeave += (s, e) => maximizeButton.BackColor = Color.Transparent;
            maximizeButton.Click += (s, e) => {
                if (this.Size == new Size(720, 540))
                {
                    // Calculate centered position
                    Rectangle workingArea = Screen.FromHandle(this.Handle).WorkingArea;
                    this.MaximumSize = new Size(1024, 768);
                    
                    // Calculate coordinates to center
                    int x = workingArea.X + (workingArea.Width - this.MaximumSize.Width) / 2;
                    int y = workingArea.Y + (workingArea.Height - this.MaximumSize.Height) / 2;
                    
                    this.Location = new Point(x, y);
                    this.Size = this.MaximumSize;
                    maximizeButton.Text = "❐";
                }
                else
                {
                    // Calculate centered position for reduced size
                    Rectangle workingArea = Screen.FromHandle(this.Handle).WorkingArea;
                    this.Size = new Size(720, 540);
                    
                    // Calculate coordinates to center
                    int x = workingArea.X + (workingArea.Width - this.Size.Width) / 2;
                    int y = workingArea.Y + (workingArea.Height - this.Size.Height) / 2;
                    
                    this.Location = new Point(x, y);
                    this.StartPosition = FormStartPosition.Manual;
                    this.WindowState = FormWindowState.Normal;
                    maximizeButton.Text = "□";
                }
            };

            minimizeButton.MouseEnter += (s, e) => minimizeButton.BackColor = Color.FromArgb(50, 50, 52);
            minimizeButton.MouseLeave += (s, e) => minimizeButton.BackColor = Color.Transparent;
            minimizeButton.Click += (s, e) => this.WindowState = FormWindowState.Minimized;

            // Add buttons in the correct order
            titleBar.Controls.Add(titleLabel);
            titleBar.Controls.Add(minimizeButton);
            titleBar.Controls.Add(maximizeButton);
            titleBar.Controls.Add(closeButton);

            // Create login controls with relative sizes and responsive layout
            Label lblUsername = new Label
            {
                Text = "Username:",
                ForeColor = Color.FromArgb(225, 225, 225),
                Font = new Font("Segoe UI", 10F),
                AutoSize = true,
                Anchor = AnchorStyles.Left
            };

            TextBox txtUsername = new TextBox
            {
                BackColor = Color.FromArgb(38, 38, 40),
                ForeColor = Color.FromArgb(225, 225, 225),
                Font = new Font("Segoe UI", 10F),
                BorderStyle = BorderStyle.FixedSingle,
                Anchor = AnchorStyles.Left | AnchorStyles.Right,
                MinimumSize = new Size(180, 25)
            };

            Label lblPassword = new Label
            {
                Text = "Password:",
                ForeColor = Color.FromArgb(225, 225, 225),
                Font = new Font("Segoe UI", 10F),
                AutoSize = true,
                Anchor = AnchorStyles.Left
            };

            TextBox txtPassword = new TextBox
            {
                BackColor = Color.FromArgb(38, 38, 40),
                ForeColor = Color.FromArgb(225, 225, 225),
                Font = new Font("Segoe UI", 10F),
                BorderStyle = BorderStyle.FixedSingle,
                PasswordChar = '•',
                Anchor = AnchorStyles.Left | AnchorStyles.Right,
                MinimumSize = new Size(180, 25)
            };

            Button btnLogin = new Button
            {
                Text = "Login",
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(147, 112, 219),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand,
                FlatAppearance = { BorderSize = 0 },
                MinimumSize = new Size(180, 35),
                Anchor = AnchorStyles.Left | AnchorStyles.Right
            };

            // Responsive layout adjusted
            TableLayoutPanel loginTable = new TableLayoutPanel
            {
                ColumnCount = 2,
                RowCount = 3,
                Dock = DockStyle.Fill,
                Padding = new Padding(10),
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                MinimumSize = new Size(300, 200)
            };

            // Configure columns with fixed minimum sizes
            loginTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 80)); 
            loginTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 180)); 

            // Configure rows with fixed spacing
            loginTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 35));
            loginTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 35));
            loginTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 45));

            // Adjust minimum sizes 
            txtUsername.MinimumSize = new Size(180, 25);
            txtPassword.MinimumSize = new Size(180, 25);
            btnLogin.MinimumSize = new Size(180, 35);

            // Add controls with margins
            loginTable.Controls.Add(lblUsername, 0, 0);
            loginTable.Controls.Add(txtUsername, 1, 0);
            loginTable.Controls.Add(lblPassword, 0, 1);
            loginTable.Controls.Add(txtPassword, 1, 1);
            loginTable.Controls.Add(btnLogin, 1, 2);

            Panel centerPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(10)
            };

            TableLayoutPanel mainLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3,
                RowCount = 3,
                MinimumSize = new Size(300, 200)
            };
            for (int i = 0; i < 3; i++)
            {
                mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
                mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33F));
            }

            mainLayout.Controls.Add(loginTable, 1, 1);

            centerPanel.Controls.Add(mainLayout);

            // Add all elements to the form
            this.Controls.AddRange(new Control[] { 
                titleBar,
                centerPanel
            });

            // Button events
            btnLogin.MouseEnter += (s, e) => btnLogin.BackColor = Color.FromArgb(132, 100, 197);
            btnLogin.MouseLeave += (s, e) => btnLogin.BackColor = Color.FromArgb(147, 112, 219);

            btnLogin.Click += (sender, e) =>
            {
                if (ValidateLogin(txtUsername.Text, txtPassword.Text))
                {
                    Form1 mainForm = new Form1();
                    this.Hide();
                    mainForm.ShowDialog();
                    this.Close();
                }
                else
                {
                    using (Form customMsgBox = new Form())
                    {
                        customMsgBox.BackColor = Color.FromArgb(28, 28, 30);
                        customMsgBox.FormBorderStyle = FormBorderStyle.None;
                        customMsgBox.StartPosition = FormStartPosition.CenterParent;
                        customMsgBox.Size = new Size(300, 150);
                        
                        //Add a panel to simulate the border
                        Panel borderPanel = new Panel
                        {
                            Dock = DockStyle.Fill,
                            BackColor = Color.Transparent,
                            BorderStyle = BorderStyle.FixedSingle //thin border
                        };
                        customMsgBox.Controls.Add(borderPanel);

                        //Error message
                        Label messageLabel = new Label
                        {
                            Text = "Invalid username or password!",
                            ForeColor = Color.FromArgb(225, 225, 225),
                            Dock = DockStyle.Fill,
                            TextAlign = ContentAlignment.MiddleCenter
                        };
                        borderPanel.Controls.Add(messageLabel);

                        // Ok button
                        Button okButton = new Button
                        {
                            Text = "OK",
                            Size = new Size(0, 35),
                            Dock = DockStyle.Bottom,
                            BackColor = Color.FromArgb(147, 112, 219),
                            ForeColor = Color.White
                        };
                        okButton.Click += (sender, e) => customMsgBox.Close();
                        borderPanel.Controls.Add(okButton);

                        customMsgBox.ShowDialog();
                    }
                }
            };

            // Make form draggable
            titleBar.MouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    ReleaseCapture();
                    SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
                }
            };
        }

        private bool ValidateLogin(string username, string password)
        {
            try
            {
                using (var connection = DatabaseConnection.GetConnection())
                {
                    connection.Open();
                    string query = "SELECT COUNT(*) FROM users WHERE username = @username AND password = @password";
                    
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@username", username);
                        command.Parameters.AddWithValue("@password", password);
                        
                        int count = Convert.ToInt32(command.ExecuteScalar());
                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database connection error: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        protected override void WndProc(ref Message m)
        {
            const int WM_NCHITTEST = 0x84;
            const int HTLEFT = 10;
            const int HTRIGHT = 11;
            const int HTTOP = 12;
            const int HTTOPLEFT = 13;
            const int HTTOPRIGHT = 14;
            const int HTBOTTOM = 15;
            const int HTBOTTOMLEFT = 16;
            const int HTBOTTOMRIGHT = 17;

            if (m.Msg == WM_NCHITTEST)
            {
                Point pos = new Point(m.LParam.ToInt32());
                pos = this.PointToClient(pos);

                int border = 10; 

                if (pos.Y <= border && pos.X <= border)
                    m.Result = (IntPtr)HTTOPLEFT;
                else if (pos.Y <= border && pos.X >= this.Width - border)
                    m.Result = (IntPtr)HTTOPRIGHT;
                else if (pos.Y >= this.Height - border && pos.X <= border)
                    m.Result = (IntPtr)HTBOTTOMLEFT;
                else if (pos.Y >= this.Height - border && pos.X >= this.Width - border)
                    m.Result = (IntPtr)HTBOTTOMRIGHT;
                else if (pos.Y <= border)
                    m.Result = (IntPtr)HTTOP;
                else if (pos.Y >= this.Height - border)
                    m.Result = (IntPtr)HTBOTTOM;
                else if (pos.X <= border)
                    m.Result = (IntPtr)HTLEFT;
                else if (pos.X >= this.Width - border)
                    m.Result = (IntPtr)HTRIGHT;
                else
                    base.WndProc(ref m);
            }
            else
                base.WndProc(ref m);
        }
    }
} 