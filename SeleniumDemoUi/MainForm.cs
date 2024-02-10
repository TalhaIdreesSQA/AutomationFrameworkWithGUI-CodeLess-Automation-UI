using Allure.Net.Commons;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using System.Xml.Linq;

namespace SeleniumDemoUi
{
    public partial class MainForm : Form
    {
        private AllureLifecycle allureLifecycle;
        public MainForm()
        {

            InitializeComponent();
            //InitializeReportTypeComboBox();
            allureLifecycle = AllureLifecycle.Instance;
            this.FormClosing += MainForm_FormClosing;
        }

        

    }
}