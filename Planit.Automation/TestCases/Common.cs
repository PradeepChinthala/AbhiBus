using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using Planit.Automation.Parameters;
using Planit.Automation.Selenium;
using Planit.Automation.Selenium.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        [TestInitialize]
        public void Initialize()
        {
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
        [TestCleanup]
        public void CleanUp()
        {
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
                    Console.WriteLine("Step : "+stepInfo);
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
                    Console.WriteLine("Step : " + stepInfo);
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
                    Console.WriteLine("Step : " + stepInfo);
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
