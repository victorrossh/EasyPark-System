using MySql.Data.MySqlClient;
using park_control.Database;

namespace park_control
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Login - Estacionamento";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Size = new Size(300, 200);

            // Create user and password fields
            TextBox txtUsername = new TextBox();
            txtUsername.Location = new Point(100, 30);
            txtUsername.Size = new Size(150, 20);

            TextBox txtPassword = new TextBox();
            txtPassword.Location = new Point(100, 70);
            txtPassword.Size = new Size(150, 20);
            txtPassword.PasswordChar = '*';

            // Labels
            Label lblUsername = new Label();
            lblUsername.Text = "Username:";
            lblUsername.Location = new Point(30, 33);

            Label lblPassword = new Label();
            lblPassword.Text = "Password:";
            lblPassword.Location = new Point(30, 73);

            // Login button
            Button btnLogin = new Button();
            btnLogin.Text = "Login";
            btnLogin.Location = new Point(100, 110);
            btnLogin.Size = new Size(100, 30);
            btnLogin.Click += (sender, e) =>
            {
                // Database verification
                if (ValidateLogin(txtUsername.Text, txtPassword.Text))
                {
                    Form1 mainForm = new Form1();
                    this.Hide();
                    mainForm.ShowDialog();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Invalid username or password!", "Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            // Add controls to the form
            this.Controls.AddRange(new Control[] { 
                txtUsername, txtPassword, lblUsername, lblPassword, btnLogin 
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