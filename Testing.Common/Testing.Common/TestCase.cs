using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Remote;
using System;
using Testing.Common;

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
        #region Internal variables
        protected TestingSection Config;
        protected static ILogger LOG = LogFactory.GetLogger();
        protected IWebDriver _driver;
        public IWebDriver driver
        {
            get
            {
                return _driver;
            }
        }
        public string BaseUrl
        {
            get
            {
                return Config.BaseUrl;
            }
        }
        #endregion

        public TestCase()
        {
            Config = (Testing.Common.TestingSection)System.Configuration.ConfigurationManager.GetSection("testing");
        }

        #region Test init and cleaup
        [TestInitialize]
        public void Setup()
        {
            var options = BuildOptions();
            BuildDriver(options);
        }

        [TestCleanup]
        public void Cleanup()
        {
            driver.Quit();
        }
        #endregion

        #region Helpers

        public DriverOptions BuildOptions()
        {
            switch (Config.Browser)
            {
                case "chrome":
                    return new ChromeOptions();
                case "ie":
                    return new InternetExplorerOptions();
                default:
                    return new FirefoxOptions();
            }
        }

        public void BuildDriver(DriverOptions options)
        {
            var localDriver = !Config.Remote.ElementInformation.IsPresent;
            
            if (localDriver)
            {
                BuildDriverLocal(options);
            }
            else
            {
                if (String.IsNullOrEmpty(Config.Remote.Url))
                    throw new Exception("Missing remote server Url");
                BuildDriverRemote(options);
            }
            _driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(localDriver ? 1 : 5));
            _driver.Manage().Timeouts().SetPageLoadTimeout(TimeSpan.FromSeconds(60));
            _driver.Manage().Timeouts().SetScriptTimeout(TimeSpan.FromSeconds(localDriver ? 1 : 5));
        }

        private void BuildDriverLocal(DriverOptions options)
        {
            switch (Config.Browser)
            {
                case "chrome":
                    _driver = new ChromeDriver();
                    break;
                case "ie":
                    _driver = new InternetExplorerDriver();
                    break;
                default:
                    {
                        FirefoxOptions firefoxOptions;
                        if (Config.Firefox.ElementInformation.IsPresent && options is FirefoxOptions)
                        {
                            firefoxOptions = (FirefoxOptions)options;
                            var profile = new FirefoxProfile();
                            profile.SetPreference(@"webdriver.firefox.bin", Config.Firefox.Path);
                            firefoxOptions.Profile = profile;
                        }
                        else
                        {
                            firefoxOptions = new FirefoxOptions();
                        }
                        _driver = new FirefoxDriver(firefoxOptions);
                    }
                    break;
            }
        }

        private void BuildDriverRemote(DriverOptions options)
        {
            _driver = new OpenQA.Selenium.Remote.RemoteWebDriver(new Uri(Config.Remote.Url), options.ToCapabilities());
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
