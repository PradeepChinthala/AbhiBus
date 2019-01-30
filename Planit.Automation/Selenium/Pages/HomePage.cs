using OpenQA.Selenium;
using Planit.Automation.Parameters;
using SeleniumExtras.PageObjects;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planit.Automation.Selenium.Pages
{
    public class HomePage : BasePage
    {
        // Constructor to initialize the elements by pagefactory
        public HomePage(IWebDriver driver): base(driver) { }


        //declaration of page objects

        [FindsBy]
        private IWebElement source = null, destination = null, datepicker1 = null, datepicker2 = null;

        [FindsBy(How = How.CssSelector,Using = ".mainheader div a img")]
        private IWebElement toolTip = null;

        [FindsBy(How = How.CssSelector, Using = "a.icosearch")]
        private IWebElement searchButton = null;

        [FindsBy(How = How.XPath, Using = "//div[@class='ui-datepicker-group ui-datepicker-group-first']")]
        private IWebElement datePicketFirst = null;

        string listPattern = "//li[text()='{0}']";
        string dateSelection = "//td[@data-handler='selectDay']/a[text()='{0}']";

        // Move to Element and The Title
        public string MoveGetToolTip()
        {
            //Wait(ExpectedConditions.ElementIsVisible(By.CssSelector(".mainheader div a img")));
            MoveElement(toolTip);
            return toolTip.GetAttribute("title").ToString();
        }


        // Search for bus
        public void EnterSearch(string source, string destination)
        {
            this.source.SendKeys(source);
            driver.FindElement(By.XPath(string.Format(listPattern, Parameter.Get<string>("Source")))).Click();  // Select option From the Bootstrap Dropdown
            this.destination.SendKeys(destination);
            driver.FindElement(By.XPath(string.Format(listPattern, Parameter.Get<string>("Destination")))).Click(); // Select option From the Bootstrap Dropdown

            datepicker1.Click();
            var getCurrentDate = Convert.ToInt32(datePicketFirst.FindElement(By.XPath("//td[contains(@class,'ui-datepicker-current-day')]/a")).Text);
            if (getCurrentDate + 2 > 30)
                getCurrentDate = 1;

            datePicketFirst.FindElement(By.XPath(string.Format(dateSelection, getCurrentDate + 2))).Click();  // Date Of Journey with Addtion (2+Currentdate)


            datepicker2.Click();
            datePicketFirst.FindElement(By.XPath(string.Format(dateSelection, getCurrentDate + 4))).Click(); // Date Of Return with Addtion (4+Currentdate)

            searchButton.Click();
            
            //// Work Around If search fail
            //if (FindBy(By.CssSelector("a.icosearch")))
            //{
            //    //Create Date Pattern
            //    var currentDate = System.DateTime.Now;
            //    string journeyDate = currentDate.AddDays(2).ToString("dd-MM-yyyy");
            //    string returnDate = currentDate.AddDays(3).ToString("dd-MM-yyyy");

            //    string Url = $"{driver.Url}bus_search/{Parameter.Get<string>("Source")}/3/{Parameter.Get<string>("Destination")}/7/{journeyDate}/R/{returnDate}";
            //    driver.Navigate().GoToUrl(Url);
            //}
        }
    }
}
