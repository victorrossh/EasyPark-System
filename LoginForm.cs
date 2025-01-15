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
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Size = new Size(320, 250);
            this.BackColor = Color.FromArgb(28, 28, 30);

            // Enable custom title bar color
            this.FormBorderStyle = FormBorderStyle.None;
            
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

            Button closeButton = new Button
            {
                Text = "×",
                Size = new Size(32, 32),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(38, 38, 40),
                ForeColor = Color.FromArgb(225, 225, 225),
                Font = new Font("Arial", 14F),
                Cursor = Cursors.Hand,
                Dock = DockStyle.Right,
                FlatAppearance = { BorderSize = 0 }
            };

            closeButton.Click += (s, e) => this.Close();
            closeButton.MouseEnter += (s, e) => closeButton.BackColor = Color.FromArgb(232, 17, 35);
            closeButton.MouseLeave += (s, e) => closeButton.BackColor = Color.FromArgb(38, 38, 40);

            titleBar.Controls.AddRange(new Control[] { titleLabel, closeButton });

            // Common style for TextBoxes
            Action<TextBox> styleTextBox = (txt) => {
                txt.BackColor = Color.FromArgb(38, 38, 40);
                txt.ForeColor = Color.FromArgb(225, 225, 225);
                txt.Font = new Font("Segoe UI", 10F);
                txt.BorderStyle = BorderStyle.FixedSingle;
                txt.Size = new Size(180, 25);
            };

            // TextBoxes
            TextBox txtUsername = new TextBox();
            txtUsername.Location = new Point(100, 70);
            styleTextBox(txtUsername);

            TextBox txtPassword = new TextBox();
            txtPassword.Location = new Point(100, 120);
            txtPassword.PasswordChar = '•';
            styleTextBox(txtPassword);

            // Labels
            Action<Label> styleLabel = (lbl) => {
                lbl.ForeColor = Color.FromArgb(225, 225, 225);
                lbl.Font = new Font("Segoe UI", 10F);
            };

            Label lblUsername = new Label();
            lblUsername.Text = "Username:";
            lblUsername.Location = new Point(20, 73);
            styleLabel(lblUsername);

            Label lblPassword = new Label();
            lblPassword.Text = "Password:";
            lblPassword.Location = new Point(20, 123);
            styleLabel(lblPassword);

            // Login button
            Button btnLogin = new Button();
            btnLogin.Text = "Login";
            btnLogin.Location = new Point(100, 170);
            btnLogin.Size = new Size(180, 35);
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.BackColor = Color.FromArgb(147, 112, 219); // Light purple
            btnLogin.ForeColor = Color.White;
            btnLogin.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnLogin.Cursor = Cursors.Hand;
            btnLogin.FlatAppearance.BorderSize = 0;

            // Button hover effect
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
                        customMsgBox.Text = "Error";
                        customMsgBox.BackColor = Color.FromArgb(28, 28, 30);
                        customMsgBox.ForeColor = Color.FromArgb(225, 225, 225);
                        customMsgBox.FormBorderStyle = FormBorderStyle.None;
                        customMsgBox.StartPosition = FormStartPosition.CenterParent;
                        customMsgBox.Size = new Size(300, 150);

                        // Barra de título personalizada para mensagem de erro
                        Panel errorTitleBar = new Panel
                        {
                            BackColor = Color.FromArgb(38, 38, 40),
                            Dock = DockStyle.Top,
                            Height = 32
                        };

                        Label errorTitle = new Label
                        {
                            Text = "Error",
                            ForeColor = Color.FromArgb(225, 225, 225),
                            Font = new Font("Segoe UI", 9F),
                            Location = new Point(10, 8),
                            AutoSize = true
                        };

                        Button errorCloseButton = new Button
                        {
                            Text = "×",
                            Size = new Size(32, 32),
                            FlatStyle = FlatStyle.Flat,
                            BackColor = Color.FromArgb(38, 38, 40),
                            ForeColor = Color.FromArgb(225, 225, 225),
                            Font = new Font("Arial", 14F),
                            Cursor = Cursors.Hand,
                            Dock = DockStyle.Right,
                            FlatAppearance = { BorderSize = 0 }
                        };

                        errorCloseButton.Click += (s, e) => customMsgBox.Close();
                        errorCloseButton.MouseEnter += (s, e) => errorCloseButton.BackColor = Color.FromArgb(232, 17, 35);
                        errorCloseButton.MouseLeave += (s, e) => errorCloseButton.BackColor = Color.FromArgb(38, 38, 40);

                        errorTitleBar.Controls.AddRange(new Control[] { errorTitle, errorCloseButton });

                        Label lblMessage = new Label
                        {
                            Text = "Invalid username or password!",
                            ForeColor = Color.FromArgb(225, 225, 225),
                            Font = new Font("Segoe UI", 10F),
                            AutoSize = true,
                            Location = new Point(30, 50)
                        };

                        Button btnOk = new Button
                        {
                            Text = "OK",
                            DialogResult = DialogResult.OK,
                            FlatStyle = FlatStyle.Flat,
                            BackColor = Color.FromArgb(147, 112, 219),
                            ForeColor = Color.White,
                            Size = new Size(80, 30),
                            Location = new Point(110, 90),
                            FlatAppearance = { BorderSize = 0 }
                        };

                        btnOk.MouseEnter += (s, ev) => btnOk.BackColor = Color.FromArgb(132, 100, 197);
                        btnOk.MouseLeave += (s, ev) => btnOk.BackColor = Color.FromArgb(147, 112, 219);

                        customMsgBox.Controls.AddRange(new Control[] { errorTitleBar, lblMessage, btnOk });
                        customMsgBox.ShowDialog(this);
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

            // Add controls to the form
            this.Controls.AddRange(new Control[] { 
                titleBar,
                txtUsername, 
                txtPassword, 
                lblUsername, 
                lblPassword, 
                btnLogin 
            });
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
    }
} 