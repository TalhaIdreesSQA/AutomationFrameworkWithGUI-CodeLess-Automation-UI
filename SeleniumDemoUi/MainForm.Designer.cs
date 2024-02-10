using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Microsoft.VisualBasic;
using System.ComponentModel.Design;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using AventStack.ExtentReports.MarkupUtils;
using AspectInjector.Broker;
using Microsoft.VisualBasic.ApplicationServices;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;
using Allure.Net.Commons;
using NUnit.Framework;

namespace SeleniumDemoUi
{
    partial class MainForm : Form
    {
        //string reportFilePath = @"C:\Users\Taha\source\repos\SeleniumDemoUi\SeleniumDemoUi\ScreenShot\";

        private System.Windows.Forms.TextBox websiteUrlTextBox;
        private System.Windows.Forms.TextBox pathlocation;
        private System.Windows.Forms.TextBox numberOfActionsTextBox;
        private System.Windows.Forms.TextBox numberOfPagesTextBox;

        private List<System.Windows.Forms.ComboBox> actionComboBoxes;
        private List<System.Windows.Forms.ComboBox> locatorTypeComboBoxes;
        private List<System.Windows.Forms.TextBox> locatorTextBoxes;
        private List<System.Windows.Forms.TextBox> sendKeysTextBoxes;

        public static IWebDriver driver;
        private static ExtentHtmlReporter htmlReporter;
        private static ExtentReports extent;
        private static ExtentTest parentTest;
        private static ExtentTest childTest;
        public int numberofactionsperpage;
        List<string> pageNames = new List<string>();

        private Dictionary<string, Dictionary<string, int>> methodActionCounts = new Dictionary<string, Dictionary<string, int>>();


        private PictureBox logoPictureBox;

        public static string LocationPath;
        private List<List<string>> methodNames = new List<List<string>>();
        public List<System.Windows.Forms.TextBox> SendKeysTextBoxes { get; }
        public string WebsiteUrl
        {
            get { return websiteUrlTextBox.Text; }
        }

        public List<string> Actions
        {
            get { return actionComboBoxes.Select(textBox => textBox.Text).ToList(); }
        }

        public List<string> Locators
        {
            get { return locatorTextBoxes.Select(textBox => textBox.Text).ToList(); }
        }

        public List<string> SendKeysValues
        {
            get { return sendKeysTextBoxes.ConvertAll(textBox => textBox.Text); }
        }

        public List<string> LocatorType
        {
            get { return locatorTypeComboBoxes.Select(textBox => textBox.Text).ToList(); }
        }



        private void InitializeComponent()
        {
            int yoffset = 230;
            logoPictureBox = new PictureBox
            {
                Location = new System.Drawing.Point(10, 10),
                Size = new System.Drawing.Size(200, 200),
                Image = System.Drawing.Image.FromFile("C:\\Users\\Taha\\source\\repos\\SeleniumDemoUi\\SeleniumDemoUi\\ScreenShot\\Logo\\Black TTL.png"), // Set your logo image here
                SizeMode = PictureBoxSizeMode.CenterImage
            };
            Controls.Add(logoPictureBox);


            System.Windows.Forms.Label headingLabel = new System.Windows.Forms.Label
            {
                Location = new System.Drawing.Point(yoffset, 40),
                Size = new System.Drawing.Size(400, 30),
                Text = "Test Tech Lab",
                Font = new System.Drawing.Font("Arial", 16, System.Drawing.FontStyle.Bold),
                TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            };
            Controls.Add(headingLabel);


            System.Windows.Forms.Label locationLabel = new System.Windows.Forms.Label
            {
                Location = new System.Drawing.Point(yoffset, 90), // Adjust Y offset as needed
                Size = new System.Drawing.Size(300, 20),
                Text = "Enter path where you can save screenshot and report"
            };
            Controls.Add(locationLabel);

            pathlocation = new System.Windows.Forms.TextBox
            {
                Location = new System.Drawing.Point(yoffset, 110),
                Size = new System.Drawing.Size(300, 20),
                PlaceholderText = "ex : C:\\User\\ScreenShot\\"
            };
            pathlocation.TextChanged += PathLocation_TextChanged;
            Controls.Add(pathlocation);

            websiteUrlTextBox = new System.Windows.Forms.TextBox
            {
                Location = new System.Drawing.Point(yoffset, 150),
                Size = new System.Drawing.Size(300, 20),
                PlaceholderText = "Enter website URL"
            };
            Controls.Add(websiteUrlTextBox);

            
            numberOfPagesTextBox = new System.Windows.Forms.TextBox
            {
                Location = new System.Drawing.Point(yoffset, 190),
                Size = new System.Drawing.Size(300, 20),
                PlaceholderText = "Enter number of Pages"
            };
            Controls.Add(numberOfPagesTextBox);


            System.Windows.Forms.Button generateFieldsButton = new System.Windows.Forms.Button
            {
                Location = new System.Drawing.Point(yoffset, 230),
                Size = new System.Drawing.Size(100, 30),
                Text = "Generate Fields",
                BackColor = System.Drawing.Color.LightCyan, // Set the background color
                ForeColor = System.Drawing.Color.Black // Set the text color
                
            };
            generateFieldsButton.Click += GenerateFieldsButtonClick;
            generateFieldsButton.MouseEnter += GenerateFieldsButton_MouseEnter; // Handle MouseEnter event
            generateFieldsButton.MouseLeave += GenerateFieldsButton_MouseLeave;

            Controls.Add(generateFieldsButton);




            actionComboBoxes = new List<System.Windows.Forms.ComboBox>();
            locatorTextBoxes = new List<System.Windows.Forms.TextBox>();
            locatorTypeComboBoxes = new List<System.Windows.Forms.ComboBox>();
            sendKeysTextBoxes = new List<System.Windows.Forms.TextBox>();
        }

        // MouseEnter event handler for Generate Fields Button
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

        private void SaveFieldsButton_MouseEnter(object sender, EventArgs e)
        {
            // Change background color to indicate hover
            ((System.Windows.Forms.Button)sender).BackColor = System.Drawing.Color.LightSeaGreen;
        }

        // MouseLeave event handler for Generate Fields Button
        private void SaveFieldsButton_MouseLeave(object sender, EventArgs e)
        {
            // Restore initial background color when mouse leaves
            ((System.Windows.Forms.Button)sender).BackColor = System.Drawing.Color.GhostWhite;
        }

        private By GetLocator(string locatorType, string locatorValue)
        {
            switch (locatorType)
            {
                case "Id":
                    return By.Id(locatorValue);
                case "Name":
                    return By.Name(locatorValue);
                case "XPath":
                    return By.XPath(locatorValue);
                case "Css Selector":
                    return By.CssSelector(locatorValue);
                case "Class Name":
                    return By.ClassName(locatorValue);
                default:
                    throw new ArgumentException("Unsupported locator type");
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // MainForm_FormClosing event code...
            MessageBox.Show("Main Form Closing");
        }

        private void GenerateFieldsButtonClick(object sender, EventArgs e)
        {
            int yOffset = 350;
            Dictionary<string, int> methodActionCount = new Dictionary<string, int>();

            if (int.TryParse(numberOfPagesTextBox.Text, out int numberOfPages) )
            {
               
                for (int j = 1; j <= numberOfPages; j++)
                {
                    string pageName = Interaction.InputBox($"Enter name for Page {j}:", "Page Name", "");
                    if (string.IsNullOrEmpty(pageName))
                    {
                        MessageBox.Show("Please enter a name for each page.");
                        return; // Exit the method if input is invalid
                    }
                    pageNames.Add(pageName);

                    int numberOfMethods;
                    if (int.TryParse(Interaction.InputBox($"Enter number of methods for Page {j}:", "Number of Methods", ""), out numberOfMethods))
                    {
                        List<string> methods = new List<string>();
                        //Dictionary<string, int> methodActionCount = new Dictionary<string, int>(); // Track number of actions for each method

                        for (int k = 1; k <= numberOfMethods; k++)
                        {
                            // Prompt user for method name
                            string methodName = Interaction.InputBox($"Enter name for Method {k} of Page {j}:", "Method Name", "");
                            if (string.IsNullOrEmpty(methodName))
                            {
                                MessageBox.Show("Please enter a name for each method.");
                                return;
                            }
                            methods.Add(methodName);

                            if (int.TryParse(Interaction.InputBox($"Enter number of actions for Page {j}:", "Number of Actions", ""), out numberofactionsperpage))
                            {
                                methodActionCount.Add(methodName, numberofactionsperpage);


                            }
                            else
                            {
                                MessageBox.Show("Please enter a valid number for the number of actions.");
                                return; // Exit the method if input is invalid
                            }

                            for (int i = 1; i <= numberofactionsperpage; i++)
                            {
                                //LogInfo(AventStack.ExtentReports.Status.Info, $"Action {i} of Method {methodName}");
                                //int yOffset = 210;
                                yOffset = actionComboBoxes.Count * 30 + 300;

                                System.Windows.Forms.ComboBox actionComboBox = new System.Windows.Forms.ComboBox
                                {
                                    Location = new System.Drawing.Point(10, yOffset),
                                    Size = new System.Drawing.Size(200, 20),
                                    DropDownStyle = ComboBoxStyle.DropDownList,
                                };
                                
                                actionComboBox.Items.AddRange(new string[] { "Write", "Click", "AssertionAreEqual", "AssertionAreNotEqual" });
                                actionComboBox.SelectedIndexChanged += ActionComboBox_SelectedIndexChanged;
                                actionComboBox.MouseEnter += ActionComboBox_MouseEnter;
                                actionComboBox.MouseLeave += ActionComboBox_MouseLeave;
                                actionComboBoxes.Add(actionComboBox);
                                Controls.Add(actionComboBox);

                                System.Windows.Forms.ComboBox locatorTypeComboBox = new System.Windows.Forms.ComboBox
                                {
                                    Location = new System.Drawing.Point(220, yOffset),
                                    Size = new System.Drawing.Size(200, 20),
                                    DropDownStyle = ComboBoxStyle.DropDownList,
                                };
                                locatorTypeComboBox.Items.AddRange(new string[] { "Id", "Name", "XPath","ClassName","CssSelector" });
                                locatorTypeComboBoxes.Add(locatorTypeComboBox);
                                locatorTypeComboBox.MouseEnter += ActionComboBox_MouseEnter;
                                locatorTypeComboBox.MouseLeave += ActionComboBox_MouseLeave;
                                Controls.Add(locatorTypeComboBox);

                                System.Windows.Forms.TextBox locatorTextBox = new System.Windows.Forms.TextBox
                                {
                                    Location = new System.Drawing.Point(430, yOffset),
                                    Size = new System.Drawing.Size(200, 20),
                                    PlaceholderText = $"Locator #{i} for Page #{j}",
                                    Visible = false // Initially hidden
                                };
                                locatorTextBoxes.Add(locatorTextBox);
                                locatorTextBox.MouseEnter += LocatorTextBox_MouseEnter;
                                locatorTextBox.MouseLeave += LocatorTextBox_MouseLeave;
                                Controls.Add(locatorTextBox);
                                

                                System.Windows.Forms.TextBox sendKeysTextBox = new System.Windows.Forms.TextBox
                                {
                                    Location = new System.Drawing.Point(640, yOffset),
                                    Size = new System.Drawing.Size(200, 20),
                                    PlaceholderText = $"SendKeys #{i} for Page #{j}",
                                    Visible = false // Initially hidden
                                };
                                sendKeysTextBoxes.Add(sendKeysTextBox);
                                sendKeysTextBox.MouseEnter += SendKeysTextBox_MouseEnter;
                                sendKeysTextBox.MouseLeave += SendKeysTextBox_MouseLeave;

                                Controls.Add(sendKeysTextBox);

                                

                                yOffset += 30;

                            }
                        }
                        methodNames.Add(methods);
                        methodActionCounts.Add(pageName, methodActionCount);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please enter a valid number for the number of pages.");
            }

            System.Windows.Forms.Button saveButton = new System.Windows.Forms.Button
            {
                Location = new System.Drawing.Point(10, yOffset),
                Size = new System.Drawing.Size(100, 30),
                Text = "Save",
                BackColor = System.Drawing.Color.LightCyan, // Set the background color
                ForeColor = System.Drawing.Color.Black
            };
            saveButton.Click += SaveButtonClick;
            saveButton.MouseEnter += SaveFieldsButton_MouseEnter; // Handle MouseEnter event
            saveButton.MouseLeave += SaveFieldsButton_MouseLeave;
            Controls.Add(saveButton);
        }




        private void SaveButtonClick(object sender, EventArgs e)
        {
            try
            {
                InitializeExtentReports();
                //StartTest("Selenium Test", "Automated test using Selenium");

                int numberOfPages;
                if (!int.TryParse(numberOfPagesTextBox.Text, out numberOfPages))
                {
                    MessageBox.Show("Please enter a valid number for the number of pages.");
                    return;
                }

                // Iterate through each page
                for (int pageNum = 1; pageNum <= numberOfPages; pageNum++)
                {
                    StartTest(pageNames[pageNum - 1], "Automated test using Selenium");

                    IWebDriver driver = new ChromeDriver();
                    driver.Navigate().GoToUrl(WebsiteUrl);

                    // Iterate through each method on the current page
                    foreach (string methodName in methodNames[pageNum - 1])
                    {
                        childTest = parentTest.CreateNode(methodName);

                        // Execute actions for the current method
                        ExecuteMethodActions(methodName, pageNum, driver);
                        driver.Quit();
                        LogInfo(AventStack.ExtentReports.Status.Pass, $"Method {methodName} performed on Page {pageNum}");

                        // Reinitialize the driver for the next method
                        driver = new ChromeDriver();
                        driver.Navigate().GoToUrl(WebsiteUrl);
                        System.Threading.Thread.Sleep(2000);
                    }

                    driver.Quit();
                    LogInfo(AventStack.ExtentReports.Status.Pass, $"All methods performed on Page {pageNum}");
                }
            }
            catch (Exception ex)
            {
                LogInfo(AventStack.ExtentReports.Status.Fail, $"Test failed: {ex.Message}");
            }
            finally
            {
                EndTest();
                this.Close();
            }
        }

        private void AddSendKeysTextBox(ManualInputForm manualInputForm, int index)
        {
            // Add a new TextBox for SendKeys
            System.Windows.Forms.TextBox sendKeysTextBox = new System.Windows.Forms.TextBox
            {
                Location = new System.Drawing.Point(330, 250 + index * 30), // Adjust the Y-coordinate based on the index
                Size = new System.Drawing.Size(150, 20),
                PlaceholderText = $"Enter SendKeys for action #{index + 1}"
            };
            Controls.Add(sendKeysTextBox);
            SendKeysTextBoxes.Add(sendKeysTextBox);
        }

        private void ActionComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            System.Windows.Forms.ComboBox actionComboBox = sender as System.Windows.Forms.ComboBox;
            int index = actionComboBoxes.IndexOf(actionComboBox);

            if (index >= 0 && index < locatorTypeComboBoxes.Count)
            {
                System.Windows.Forms.ComboBox locatorTypeComboBox = locatorTypeComboBoxes[index];
                System.Windows.Forms.TextBox locatorTextBox = locatorTextBoxes[index];
                System.Windows.Forms.TextBox sendKeysTextBox = sendKeysTextBoxes[index];

                if (actionComboBox.SelectedItem?.ToString().ToLower() == "write" || actionComboBox.SelectedItem?.ToString().ToLower() == "click" || actionComboBox.SelectedItem?.ToString().ToLower() == "assertionareequal" || actionComboBox.SelectedItem?.ToString().ToLower() == "assertionarenotequal")
                {
                    locatorTypeComboBox.Visible = true;
                    locatorTextBox.Visible = true; ;
                    if (actionComboBox.Text.ToLower() == "write" || actionComboBox.Text.ToLower() == "assertionareequal" || actionComboBox.Text.ToLower() == "assertionarenotequal")
                    {
                        sendKeysTextBox.Visible = true;
                    }
                    else
                    {
                        sendKeysTextBox.Visible = false;
                    }
                }
                else
                {
                    locatorTypeComboBox.Visible = false;
                    locatorTextBox.Visible = false;
                    sendKeysTextBox.Visible = false;
                }
            }
        }

        

        public static void CaptureScreenshot(IWebDriver driver, string fileName)
        {
            // Take screenshot
            Screenshot screenshot = ((ITakesScreenshot)driver).GetScreenshot();
            Thread.Sleep(2000);
            // Save screenshot
            string screenshotDirectory = LocationPath; // Change this path as needed
            if (!Directory.Exists(screenshotDirectory))
            {
                Directory.CreateDirectory(screenshotDirectory);
            }
            string screenshotPath = Path.Combine(screenshotDirectory, fileName);
            screenshot.SaveAsFile(screenshotPath, ScreenshotImageFormat.Png);
        }



        private static void InitializeExtentReports()
        {
            //AllureLifecycle.Instance.CleanupResultDirectory();

            htmlReporter = new ExtentHtmlReporter(LocationPath);
            extent = new ExtentReports();
            extent.AttachReporter(htmlReporter);
        }

        // Add this method to start a new test
        //private static void StartTest(string testName, string testDescription)
        //{
        //    parentTest = extent.CreateTest(testName, testDescription);
        //}
        private static void StartTest(string pageName, string testDescription)
        {
            parentTest = extent.CreateTest($"{pageName}", testDescription);
        }
        // Add this method to log information to the extent report
        private static void LogInfo(AventStack.ExtentReports.Status logStatus, string message)
        {
            parentTest.Log(logStatus, MarkupHelper.CreateLabel(message, ExtentColor.Red));
        }

        // Add this method to end the test and flush the report
        private static void EndTest()
        {
            extent.Flush();
        }

        private static void LogInfo(AventStack.ExtentReports.Status logStatus, string message, MediaEntityModelProvider mediaEntity)
        {
            parentTest.Log(logStatus, message, mediaEntity);
        }

        public static void ChildNode(List<string> methodNames)
        {
            foreach (string methodName in methodNames)
            {
                childTest = parentTest.CreateNode(methodName);
            }
        }



        private void ExecuteMethodActions(string methodName, int pageNum, IWebDriver driver)
        {
            // Find the index of the method in methodNames list
            int methodIndex = methodNames[pageNum - 1].IndexOf(methodName);

            // Get the corresponding list of actions for the method
            List<System.Windows.Forms.ComboBox> methodActions = actionComboBoxes.GetRange(methodIndex * numberofactionsperpage, numberofactionsperpage);
            List<System.Windows.Forms.ComboBox> methodLocatorTypes = locatorTypeComboBoxes.GetRange(methodIndex * numberofactionsperpage, numberofactionsperpage);
            List<System.Windows.Forms.TextBox> methodLocators = locatorTextBoxes.GetRange(methodIndex * numberofactionsperpage, numberofactionsperpage);
            List<System.Windows.Forms.TextBox> methodSendKeys = sendKeysTextBoxes.GetRange(methodIndex * numberofactionsperpage, numberofactionsperpage);

            // Perform actions related to the method
            foreach (var actionComboBox in methodActions)
            {
                string action = actionComboBox.Text;
                string locator = methodLocators[methodActions.IndexOf(actionComboBox)].Text;
                string sendKeys = methodSendKeys[methodActions.IndexOf(actionComboBox)].Text;
                string locatorType = methodLocatorTypes[methodActions.IndexOf(actionComboBox)].Text;

                if (action.ToLower() == "write")
                {
                    By loc = GetLocator(locatorType, locator);
                    IWebElement element = driver.FindElement(loc);
                    element.SendKeys(sendKeys);
                    CaptureScreenshot(driver, $"{methodName}_Write_Action.png");
                    childTest.Log(AventStack.ExtentReports.Status.Pass, $"Write Action: {locator} - Value: {sendKeys}", MediaEntityBuilder.CreateScreenCaptureFromPath($"{LocationPath}{methodName}_Write_Action.png").Build());
                }
                else if (action.ToLower() == "click")
                {
                    By loc = GetLocator(locatorType, locator);
                    IWebElement element = driver.FindElement(loc);
                    element.Click();
                    CaptureScreenshot(driver, $"{methodName}_Click_Action.png");
                    childTest.Log(AventStack.ExtentReports.Status.Fail, $"Click Action: {locator}", MediaEntityBuilder.CreateScreenCaptureFromPath($"{LocationPath}{methodName}_Click_Action.png").Build());
                }
                else if (action.ToLower() == "assertionareequal")
                {
                    By loc = GetLocator(locatorType,locator);
                    string element = driver.FindElement(loc).Text;
                    Assert.AreEqual(sendKeys, element);
                    CaptureScreenshot(driver, $"{methodName}_AssertAreEqual_Action.png");
                    childTest.Log(AventStack.ExtentReports.Status.Pass, $"AssertAreEqual Action: {locator}", MediaEntityBuilder.CreateScreenCaptureFromPath($"{LocationPath}{methodName}_AssertAreEqual_Action.png").Build());
                }
                else if (action.ToLower() == "assertionarenotequal")
                {
                    By loc = GetLocator(locatorType, locator);
                    string element = driver.FindElement(loc).Text;
                    Assert.AreNotEqual(sendKeys, element);
                    CaptureScreenshot(driver, $"{methodName}_AssertNotEqual_Action.png");
                    childTest.Log(AventStack.ExtentReports.Status.Pass, $"AssertNotEqual Action: {locator}", MediaEntityBuilder.CreateScreenCaptureFromPath($"{LocationPath}{methodName}_AssertNotEqual_Action.png").Build());
                }
                

            }
        }

        public void ActionComboBox_MouseEnter(object sender, EventArgs e)
        {
            ((System.Windows.Forms.ComboBox)sender).BackColor = System.Drawing.Color.LightSeaGreen;
        }

        public void ActionComboBox_MouseLeave(object sender, EventArgs e)
        {
            ((System.Windows.Forms.ComboBox)sender).BackColor = System.Drawing.Color.White; // Set the initial background color
        }
        private void LocatorTextBox_MouseEnter(object sender, EventArgs e)
        {
            ((System.Windows.Forms.TextBox)sender).BackColor = System.Drawing.Color.LightSeaGreen;
        }
        private void LocatorTextBox_MouseLeave(object sender, EventArgs e)
        {
            ((System.Windows.Forms.TextBox)sender).BackColor = System.Drawing.Color.White; // Set the initial background color
        }

        private void SendKeysTextBox_MouseEnter(object sender, EventArgs e)
        {
            ((System.Windows.Forms.TextBox)sender).BackColor = System.Drawing.Color.LightSeaGreen;
        }
        private void SendKeysTextBox_MouseLeave(object sender, EventArgs e)
        {
            ((System.Windows.Forms.TextBox)sender).BackColor = System.Drawing.Color.White; // Set the initial background color
        }

        private void PathLocation_TextChanged(object sender, EventArgs e)
        {
            LocationPath = pathlocation.Text;
        }



    }



}