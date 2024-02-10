namespace SeleniumDemoUi
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //// To customize application configuration such as set high DPI settings or default font,
            //// see https://aka.ms/applicationconfiguration.
            //////ApplicationConfiguration.Initialize();
            //////Application.Run(new MainForm());

            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);

            //// Create an instance of MainForm
            //MainForm mainForm = new MainForm();

            //// Run the application
            //Application.Run(mainForm);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Create an instance of LoginPage
            LoginPage loginPage = new LoginPage();

            // Run the LoginPage
            Application.Run(loginPage);

            // After the login, check if the login was successful
            if (loginPage.IsLoginSuccessful)
            {
                // Create an instance of MainForm
                MainForm mainForm = new MainForm();

                // Run the MainForm
                Application.Run(mainForm);
            }
        }
    }
}