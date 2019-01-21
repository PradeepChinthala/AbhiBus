using OpenQA.Selenium;
using Planit.Automation.Parameters;
using Planit.Automation.Selenium;
using Planit.Automation.Selenium.Pages;
using System;
using System.Collections.Generic;
using NUnit.Framework;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using AventStack.ExtentReports.Reporter.Configuration;
using NUnit.Framework.Interfaces;
using System.IO;

namespace Planit.Automation.TestCases
{
    public class Common
    {
        #region fields
        protected IWebDriver driver;
        private Exception _exception;
        private HomePage _homePage;
        private SearchPage _searchPage;
        private PaymentPage _paymentPage;

        protected static ExtentReports extent;
        protected static ExtentHtmlReporter htmlReporter;
        protected static ExtentTest test;
        #endregion

        #region Properties
        public HomePage HomePage
        {
            get
            {
                if(driver!=null)
                    _homePage =  new HomePage(driver);
                return _homePage;
            }
        }
        public SearchPage SearchPage
        {
            get
            {
                if (driver != null)
                    _searchPage = new SearchPage(driver);
                return _searchPage;
            }
        }
        public PaymentPage PaymentPage
        {
            get
            {
                if (driver != null)
                    _paymentPage = new PaymentPage(driver);
                return _paymentPage;
            }
        }
        #endregion

        #region Pre-Requisit
        [OneTimeSetUp]
        public void SetupReporting()
        {
            string directory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory.Replace(@"bin\Debug\","") + @"Reports\report.html");
            htmlReporter = new ExtentHtmlReporter(directory);

            htmlReporter.Config.Theme = Theme.Dark;
            htmlReporter.Config.DocumentTitle = "Document";
            htmlReporter.Config.ReportName = "Test Reuslt Report";

            /*htmlReporter.Configuration().JS = "$('.brand-logo').text('test image').prepend('<img src=@"file:///D:\Users\jloyzaga\Documents\FrameworkForJoe\FrameworkForJoe\Capgemini_logo_high_res-smaller-2.jpg"> ')";*/
            htmlReporter.Config.JS = "$('.brand-logo').text('').append('<img src=D:\\Users\\jloyzaga\\Documents\\FrameworkForJoe\\FrameworkForJoe\\Capgemini_logo_high_res-smaller-2.jpg>')";
            extent = new ExtentReports();
            extent.AttachReporter(htmlReporter);
        }

        [OneTimeTearDown]
        public void GenerateReport()
        {
            extent.Flush();
        }

        [SetUp]
        public void Initialize()
        {
            test = extent.CreateTest(TestContext.CurrentContext.Test.Name);

            // Collecting the Parmaeters
            /// <param name="fileName">Name of the parameter xml file.</param>
            /// <param name="criteria">IXmlParameterOptions instance. Provides sections that need to be collected from provided parameter xml file.</param>

            string fileName = "Parameters.xml";
            var criteria = new List<string>() { "SHARED" }; // Provides sections that need to be collected from provided parameter xml file
            Parameter.Collect(fileName, criteria);


            // Lanching the webdriver based on the driver type provided in the xml file
            driver = WebDriver.InitDriver();
        }
        #endregion

        #region CleanUp
        [TearDown]
        public void CleanUp()
        { 
            var status = TestContext.CurrentContext.Result.Outcome.Status;
            var stacktrace = string.IsNullOrEmpty(TestContext.CurrentContext.Result.StackTrace)
                    ? ""
                    : string.Format("{0}", TestContext.CurrentContext.Result.StackTrace);
            Status logstatus;

            switch (status)
                {
                case TestStatus.Failed:
                    logstatus = Status.Fail;
                    break;
                case TestStatus.Inconclusive:
                    logstatus = Status.Warning;
                    break;
                case TestStatus.Skipped:
                    logstatus = Status.Skip;
                    break;
                default:
                    logstatus = Status.Pass;
                    break;
                }

            test.Log(logstatus, "Test ended with " + logstatus + stacktrace);
            if (driver != null)
                driver.Quit();
        }
        #endregion

        #region methods
        public void RunStep(Action action, string stepInfo)
        {
            try
            {
                if (_exception==null)
                {
                    action();
                    test.Log(Status.Pass, "Step : " + stepInfo);
                }
            }
            catch(Exception e)
            {
                _exception = e;
                throw new Exception("Exception : " + e.Message);
            }
            
        }

        public void RunStep<T>(Action<T> action,T parmaeter ,string stepInfo)
        {
            try
            {
                if (_exception == null)
                {
                    action(parmaeter);
                    test.Log(Status.Pass, "Step : " + stepInfo);
                }
            }
            catch (Exception e)
            {
                _exception = e;
                throw new Exception("Exception : " + e.Message);
            }

        }
        public void RunStep<T>(Action<T,T> action, T parmaeter1, T parmaeter2, string stepInfo)
        {
            try
            {
                if (_exception == null)
                {
                    action(parmaeter1, parmaeter2);
                    test.Log(Status.Pass, "Step : " + stepInfo);
                }
            }
            catch (Exception e)
            {
                _exception = e;
                throw new Exception("Exception : " + e.Message);
            }

        }
        #endregion
    }
}
