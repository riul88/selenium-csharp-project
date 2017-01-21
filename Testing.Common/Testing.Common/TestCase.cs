using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Remote;
using System;

//Copyright (C) 2017 Raul Robledo <raul.robledo at acm.org>
//
//This program is free software: you can redistribute it and/or modify
//it under the terms of the GNU General Public License as published by
//the Free Software Foundation, either version 3 of the License, or
//(at your option) any later version.
//
//This program is distributed in the hope that it will be useful,
//but WITHOUT ANY WARRANTY; without even the implied warranty of
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//GNU General Public License for more details.
//
//You should have received a copy of the GNU General Public License
//along with this program.  If not, see <http://www.gnu.org/licenses/>.
namespace Testing.Common
{
    public class TestCase
    {
        #region Configurable variables

        protected const string baseUrl = @"http://gitlab:81";//@"http://int.raulrobledo.com"
        protected const string firefoxPath = @"C:\Program Files\Mozilla Firefox 47\firefox.exe";
        protected const Browser capabilities = Browser.Firefox;
        protected const bool useRemote = false;
        protected const string remoteUrl = null;

        #endregion

        #region Internal variables
        protected static ILogger LOG = LogFactory.GetLogger();
        private IWebDriver _driver;
        public IWebDriver driver
        {
            get
            {
                return _driver;
            }
        }
        public enum Browser {
            Firefox,
            Chrome,
            IE
        };
        #endregion

        #region Test init and cleaup
        [TestInitialize]
        public void Setup()
        {
            BuildDriver(!useRemote, DesiredCapabilities.Firefox(), remoteUrl);
        }

        [TestCleanup]
        public void Cleanup()
        {
            driver.Quit();
        }
        #endregion

        #region Helpers

        public void BuildDriver(bool localDriver, DesiredCapabilities capabilities, string remoteUrl = null)
        {
            capabilities.IsJavaScriptEnabled = true;
            if (localDriver)
            {
                BuildDriverLocal(capabilities);
            }
            else
            {
                if (String.IsNullOrEmpty(remoteUrl))
                    throw new Exception("Missing remote server Url");
                BuildDriverRemote(capabilities, remoteUrl);
            }
            _driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(localDriver ? 1 : 5));
            _driver.Manage().Timeouts().SetPageLoadTimeout(TimeSpan.FromSeconds(60));
            _driver.Manage().Timeouts().SetScriptTimeout(TimeSpan.FromSeconds(localDriver ? 1 : 5));
        }

        private void BuildDriverLocal(DesiredCapabilities capabilities)
        {
            if (capabilities.BrowserName == "chrome")
            {
                _driver = new ChromeDriver();
            }
            else if (capabilities.BrowserName == "ie")
            {
                _driver = new InternetExplorerDriver();
            }
            else
            {
                var profile = new FirefoxProfile();
                profile.SetPreference(@"webdriver.firefox.bin", firefoxPath);
                //var binary = new FirefoxBinary(firefoxPath);
                _driver = new FirefoxDriver(profile);
            }
        }

        private void BuildDriverRemote(DesiredCapabilities capabilities, string remoteUrl)
        {
            _driver = new OpenQA.Selenium.Remote.RemoteWebDriver(new Uri(remoteUrl), capabilities);
        }

        public bool IsElementPresent(By by)
        {
            try
            {
                driver.FindElement(by);
                return true;
            }
            catch { }
            return false;
        }
        //<script type="text/javascript">
        //    window.onerror=function(msg){
        //        document.body.setAttribute("JSError", msg);
        //    }
        //</script>
        public void AssertNoJavaScriptError()
        {
            Assert.IsFalse(IsElementPresent(By.XPath("//body[@JSError]")));
        }

        public IWebElement Find(By by, string label = null)
        {
            try
            {
                return driver.FindElement(by);
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("Unable to find element {0}", label), ex);
            }
        }

        public void Click(By by, string label = null)
        {
            Click(Find(by, label), label);
            AssertNoJavaScriptError();
        }

        public void Click(IWebElement element, string label = null)
        {
            try
            {
                element.Click();
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("Unable to click element {0}", label), ex);
            }
        }

        public void MouseOver(By by, string label = null)
        {
            MouseOver(Find(by), label);
        }

        public void MouseOver(IWebElement element, string label = null)
        {
            if (driver is RemoteWebDriver)
            {
                throw new Exception("MouseOver not available for RemoteWebDriver");
            }

            try
            {
                var action = new Actions(driver);
                action.MoveToElement(element).Perform();
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("Unable to move to element {0}", label), ex);
            }
        }

        #endregion
    }
}
