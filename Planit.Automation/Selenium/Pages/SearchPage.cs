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
        
        // Element Declaration
        [FindsBy(How = How.XPath, Using = "//div[@id='SerVicesDetOneway1']/div/div")]
        private IList<IWebElement> onWardsTravelList = null;

        [FindsBy(How = How.XPath, Using = "//div[@id='SerVicesDetOneway2']/div/div")]
        private IList<IWebElement> returnTravelList = null;

        [FindsBy(How = How.CssSelector, Using = "span#totalfare")]
        private IWebElement amount = null;


        // Resuable strings
        string SelectSeat = "//div[@class='col3 booksts clearfix']//a[@class='btnab book1']";
        string EmptySeat = "//div[5]//li[contains(@class,' available')]/a";
        string DropDown = "//div[5]//select";
        string Submit = "//div[5]//input[@type='submit']";


        // Book OnWards and Return Ticket
        public void BookTicket()
        {
            string str= string.Empty;
            ScreenOverlay();  //Wiat for screen load
            foreach (var o in onWardsTravelList)
                try
                {
                    o.FindElement(By.XPath(SelectSeat)).Click();   // Click on SELECT SEAT Button 
                    o.FindElements(By.XPath(EmptySeat)).FirstOrDefault().Click();  // Select available seat                        
                    SelectDropDown(o.FindElement(By.XPath(DropDown)), Parameter.Get<string>("Boarding"),ref str);   // Select Boarding Point                 
                    Parameter.Add("OnwardsAmount", amount.Text.Replace(" ", ""));    // Store Onwards Amount into Dicationary
                    o.FindElement(By.XPath(Submit)).Click();  // Book return button
                    ScreenOverlay();  //Wiat for screen load

                    // Book Return Journey
                    for (int i = 0; i < returnTravelList.Count; i++)
                        try
                        {
                            // Avoid StaleElementReference Exception
                            staleElement(By.XPath($"//div[@id='SerVicesDetOneway2']/div/div[{i + 1}]{SelectSeat}")).Click(); // Click on SELECT SEAT Button 
                            staleElement(By.XPath($"//div[@id='SerVicesDetOneway2']/div/div[{i + 1}]{EmptySeat}")).Click(); // Select available seat 
                            var element = staleElement(By.XPath($"//div[@id='SerVicesDetOneway2']/div/div[{i + 1}]//span[@id='totalfare']"));
                            Parameter.Add("ReturnAmount", element.Text.Replace(" ", ""));  // Store Return Amount into Dicationary
                            SelectDropDown(staleElement(By.XPath($"//div[@id='SerVicesDetOneway2']/div/div[{i + 1}]{DropDown}")), Parameter.Get<string>("ReturnBoarding"),ref str);// Select Boarding Point  

                            if(str!=string.Empty) // if borading point is not in the list
                                Parameter.Add("ReturnBoarding",str);

                            staleElement(By.XPath($"//div[@id='SerVicesDetOneway2']/div/div[{i + 1}]{Submit}")).Click();
                            break;
                        }
                        catch { }

                    break;
                }
                catch { }
        }
    }
}
