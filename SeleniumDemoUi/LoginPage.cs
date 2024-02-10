using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumDemoUi
{
    internal class LoginPage : Form
    {
        private TextBox usernameTextBox;
        private TextBox passwordTextBox;
        private Button loginButton;
        private System.Windows.Forms.Timer textAnimationTimer;
        private readonly string scrollingText = "Tech Tech Lab";

        private int textPositionX;
        

        public bool IsLoginSuccessful { get; private set; }
        public LoginPage()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            // Initialize UI components, set properties, and add event handlers
            // ...

            // Example UI components
            usernameTextBox = new TextBox
            {
                Location = new System.Drawing.Point(50, 50),
                Size = new System.Drawing.Size(150, 20),
                PlaceholderText = "Username"
            };
            Controls.Add(usernameTextBox);

            passwordTextBox = new TextBox
            {
                Location = new System.Drawing.Point(50, 80),
                Size = new System.Drawing.Size(150, 20),
                PlaceholderText = "Password",
                PasswordChar = '*'
            };
            Controls.Add(passwordTextBox);

            loginButton = new Button
            {
                Location = new System.Drawing.Point(50, 110),
                Size = new System.Drawing.Size(100, 30),
                Text = "Login"
            };
            loginButton.Click += LoginButtonClick;
            loginButton.MouseEnter += GenerateFieldsButton_MouseEnter; // Handle MouseEnter event
            loginButton.MouseLeave += GenerateFieldsButton_MouseLeave;
            Controls.Add(loginButton);

            textAnimationTimer = new System.Windows.Forms.Timer
            {
                Interval = 50 // Adjust the interval for smooth animation
            };
            textAnimationTimer.Tick += TextAnimationTimer_Tick;
            textAnimationTimer.Start();

            
        }

        private void TextAnimationTimer_Tick(object sender, EventArgs e)
        {
            // Increase the scrolling speed for a smoother effect
            int scrollSpeed = 2;

            // Update the text position
            textPositionX += scrollSpeed;

            // Check if the text reaches the right boundary
            if (textPositionX >= ClientSize.Width)
            {
                // Reset the text position to the left
                textPositionX = -scrollingText.Length * 10;
            }

            Invalidate(); // Trigger OnPaint event
        }






        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            // Draw the scrolling text on the form
            e.Graphics.DrawString(scrollingText, Font, Brushes.Black, new Point(textPositionX, 10));

            // Draw particles
        }



        private void LoginButtonClick(object sender, EventArgs e)
        {
            // Example login logic
            string enteredUsername = usernameTextBox.Text;
            string enteredPassword = passwordTextBox.Text;

            // Replace this with your actual login logic
            if (IsValidLogin(enteredUsername, enteredPassword))
            {
                IsLoginSuccessful = true;
                // Open the main form
                MainForm mainForm = new MainForm();
                mainForm.Show();

                // Optionally, you can close the login form if needed
                this.Close();
            }
            else
            {
                MessageBox.Show("Invalid username or password. Please try again.");
            }
        }


        private bool IsValidLogin(string username, string password)
        {
            // Example: Check if the entered credentials are valid
            return username == "admin" && password == "admin";
        }

        private void GenerateFieldsButton_MouseEnter(object sender, EventArgs e)
        {
            // Change background color to indicate hover
            ((System.Windows.Forms.Button)sender).BackColor = System.Drawing.Color.LightSeaGreen;
        }

        // MouseLeave event handler for Generate Fields Button
        private void GenerateFieldsButton_MouseLeave(object sender, EventArgs e)
        {
            // Restore initial background color when mouse leaves
            ((System.Windows.Forms.Button)sender).BackColor = System.Drawing.Color.GhostWhite;
        }
    }

    
}