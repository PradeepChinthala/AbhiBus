using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.PageObjects;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Planit.Automation.Selenium.Pages
{
    public abstract class BasePage
    {
        protected IWebDriver driver;
        protected Actions actions;
        protected WebDriverWait wait;
        protected IJavaScriptExecutor js;
        public BasePage(IWebDriver driver)
        {
            this.driver = driver;
            PageFactory.InitElements(driver, this);
            actions = new Actions(driver);
            js = (IJavaScriptExecutor)driver;
        }

        // Wait Seconds can be changed in the runtime based on the requirement by default it is 20
        public void Wait<TResult>(Func<IWebDriver, TResult> condition, int seconds = 20)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(seconds));
            wait.PollingInterval = TimeSpan.FromMilliseconds(500);
            wait.IgnoreExceptionTypes(typeof(Exception));
            wait.Until(condition);
        }

        protected void MoveElement(IWebElement element)
        {
            actions.MoveToElement(element).Build().Perform();
        }

        // To Overcome Scree overlay issue
        protected void ScreenOverlay()
        {
            Wait(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//div[@class='subpage-content search-subpage-content']/span")));
        }

        // To Overcome StaleElementReferenceException issue
        public IWebElement staleElement(By by)
        {
            try
            {
                Wait(ExpectedConditions.ElementExists(by));
                return driver.FindElement(by);
            }
            catch (StaleElementReferenceException e)
            {
                return staleElement(by);
            }            
        }

        protected void SelectDropDown(IWebElement element, string option,ref string opt)
        {
            bool found = false;
            var options = element.FindElements(By.TagName("option"));
            foreach (var a in options)
            {
                if (a.Text.Contains(option))
                {
                    a.Click();
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                options[2].Click();
                opt = options[2].Text.Split('-')[0];
            }
        }
    }
}
