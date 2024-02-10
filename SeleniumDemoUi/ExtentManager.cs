using AventStack.ExtentReports.Reporter;
using AventStack.ExtentReports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.ApplicationServices;
using OpenQA.Selenium;
using System.IO;
using System.Reflection.Metadata;
using AventStack.ExtentReports.MarkupUtils;

namespace SeleniumDemoUi
{
    internal class ExtentManager
    {
        

        private static ExtentHtmlReporter htmlReporter;
        private static ExtentReports extent;
        private static ExtentTest parentTest;
        private static ExtentTest childTest;
        public static string path = "C:\\Users\\Taha\\source\\repos\\SeleniumDemoUi\\SeleniumDemoUi\\ScreenShot\\";



        private static void InitializeExtentReports()
        {
            htmlReporter = new ExtentHtmlReporter(@"C:\Users\Taha\source\repos\SeleniumDemoUi\SeleniumDemoUi\ScreenShot\ExtentReport.html");
            extent = new ExtentReports();
            extent.AttachReporter(htmlReporter);
        }

        // Add this method to start a new test
        private static void StartTest(string testName, string testDescription)
        {
            parentTest = extent.CreateTest(testName, testDescription);
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

    }
}
