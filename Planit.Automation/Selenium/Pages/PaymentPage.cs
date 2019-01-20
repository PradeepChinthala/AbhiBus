using OpenQA.Selenium;
using Planit.Automation.Parameters;
using SeleniumExtras.PageObjects;
using System;
using System.Collections.Generic;

namespace Planit.Automation.Selenium.Pages
{
    public class PaymentPage : BasePage
    {
        // Constructor to initialize the elements by pagefactory
        public PaymentPage(IWebDriver driver) : base(driver) { }

        [FindsBy(How = How.XPath, Using = "//div[@class='detailjrny'][1]")]
        private IWebElement onWardsJourney = null;

        [FindsBy(How = How.XPath, Using = "//div[@class='detailjrny'][2]")]
        private IWebElement returnJourney = null;

        [FindsBy(How = How.Id, Using = "NetAmountmsg")]
        private IWebElement totalFair = null;


        public JourneyDetails GetOnWardsDetails()
        {
            //var result = new List<string>();
            //result.Add(onWardsJourney.FindElement(By.XPath("//div[1]/h3/strong")).Text.Trim().Split(' ')[0]);
            //result.Add(onWardsJourney.FindElement(By.XPath("//div[1]/p[2]/strong")).Text.Trim());
            //return result;
            var details = new JourneyDetails();
            details.RoutName = onWardsJourney.FindElement(By.XPath("//div[1]/h3/strong")).Text.Trim().Split(' ')[0];
            details.Boarding = onWardsJourney.FindElement(By.XPath("//div[1]/p[2]/strong")).Text.Trim();
            details.Amount = Parameter.Get<string>("OnwardsAmount");
            return details;
        }

        public JourneyDetails GetReturnDetails()
        {
            //var result = new List<string>();
            //result.Add(returnJourney.FindElement(By.XPath("//div[1]/h3/strong")).Text.Trim().Split(' ')[0]);
            //result.Add(returnJourney.FindElement(By.XPath("//div[1]/p[2]/strong")).Text.Trim());
            //return result;
            var details = new JourneyDetails();
            details.RoutName = returnJourney.FindElement(By.XPath("//div[1]/h3/strong")).Text.Trim().Split(' ')[0];
            details.Boarding = returnJourney.FindElement(By.XPath("//div[1]/p[2]/strong")).Text.Trim();
            details.Amount = Parameter.Get<string>("ReturnAmount");
            return details;
        }

        public double GetTotalFiar()
        {
            return Convert.ToDouble(totalFair.Text.Trim().Replace(",", ""));
        }
    }
    public class JourneyDetails
    {
        public JourneyDetails()
        {
        }
        public string RoutName { get; set; }
        public string Boarding { get; set; }
        public string Amount { get; set; }
    }
}
