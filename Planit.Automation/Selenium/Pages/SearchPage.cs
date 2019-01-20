using OpenQA.Selenium;
using Planit.Automation.Parameters;
using SeleniumExtras.PageObjects;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Planit.Automation.Selenium.Pages
{
    public class SearchPage : BasePage
    {
        // Constructor to initialize the elements by pagefactory
        public SearchPage(IWebDriver driver) : base(driver) { }
        

        [FindsBy(How = How.XPath, Using = "//div[@id='SerVicesDetOneway1']/div/div")]
        private IList<IWebElement> onWardsJourney = null;

        [FindsBy(How = How.XPath, Using = "//div[@id='SerVicesDetOneway2']/div/div")]
        private IList<IWebElement> returnTravelList = null;

        [FindsBy(How = How.CssSelector, Using = "span#totalfare")]
        private IWebElement amount = null;

        string SelectSeat = "//div[@class='col3 booksts clearfix']//a[@class='btnab book1']";
        string EmptySeat = "//div[5]//li[contains(@class,' available')]/a";
        string DropDown = "//div[5]//select";
        string Submit = "//div[5]//input[@type='submit']";

        public void BookTicket()
        {
            ScreenOverlay();  //Wiat for screen load
            foreach (var o in onWardsJourney)
            {
                o.FindElement(By.XPath(SelectSeat)).Click();   // Click on SELECT SEAT Button              
                try
                {
                    o.FindElements(By.XPath(EmptySeat)).FirstOrDefault().Click();  // Select available seat                        
                    SelectDropDown(o.FindElement(By.XPath(DropDown)), Parameter.Get<string>("Boarding"));                    
                    Parameter.Add("OnwardsAmount",amount.Text.Replace(" ", ""));
                    o.FindElement(By.XPath(Submit)).Click();  // Book return button
                    ScreenOverlay();
                    // Book Return Journey
                    for (int i = 0; i < returnTravelList.Count; i++)
                    {
                        try
                        {
                            staleElement(By.XPath($"//div[@id='SerVicesDetOneway2']/div/div[{i + 1}]{SelectSeat}")).Click();
                            staleElement(By.XPath($"//div[@id='SerVicesDetOneway2']/div/div[{i + 1}]{EmptySeat}")).Click();
                            var element = staleElement(By.XPath($"//div[@id='SerVicesDetOneway2']/div/div[{i + 1}]//span[@id='totalfare']"));
                            Parameter.Add("ReturnAmount", element.Text.Replace(" ", ""));
                            SelectDropDown(staleElement(By.XPath($"//div[@id='SerVicesDetOneway2']/div/div[{i + 1}]{DropDown}")), Parameter.Get<string>("ReturnBoarding"));
                            staleElement(By.XPath($"//div[@id='SerVicesDetOneway2']/div/div[{i + 1}]{Submit}")).Click();
                            break;
                        }
                        catch { }
                    }
                    break;
                }
                catch { }
            }
        }
    }
}
