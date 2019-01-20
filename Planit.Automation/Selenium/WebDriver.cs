using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;
using Planit.Automation.Parameters;
using System;
using System.Collections.Generic;

namespace Planit.Automation.Selenium
{
    public static class WebDriver
    {
        public static IWebDriver driver;
        public static IWebDriver InitDriver(Cookie seleniumCookie = null)
        {
            string browser = Parameter.Get<string>("WebDriver");
            switch (browser)
            {
                case "chrome":
                    {
                        var chromeOptions = new ChromeOptions();
                        chromeOptions.AddUserProfilePreference("download.prompt_for_download", true);
                        chromeOptions.AddArguments("--disable-popup-blocking");
                        chromeOptions.AddArguments("--disable-print-preview");
                        chromeOptions.AddArguments("--disable-extensions");
                        chromeOptions.AddArgument("--disable-infobars");
                        chromeOptions.AddArgument("--disable-notifications");
                        ChromeDriverService service = ChromeDriverService.CreateDefaultService();
                        driver = new ChromeDriver(service, chromeOptions, TimeSpan.FromSeconds(60));
                        break;
                    }
                case "ie":
                    {

                        InternetExplorerOptions ieOptions = new InternetExplorerOptions();
                        ieOptions.IgnoreZoomLevel = true;
                        //this.IEConfiguration();                        
                        InternetExplorerDriverService service = InternetExplorerDriverService.CreateDefaultService();
                        driver = new InternetExplorerDriver(service, ieOptions, TimeSpan.FromSeconds(60));
                        break;
                    }               
                default:
                    throw new ArgumentException($"Browser Option {browser} Is Not Valid - Use Chrome, Edge or IE Instead");

            }
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl(Parameter.Get<string>("Url"));
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(Convert.ToDouble(Parameter.Get<string>("ElementTimeOut")));
            driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(Convert.ToDouble(Parameter.Get<string>("PageLoadOut")));            
            return driver;
        }
    }
}
