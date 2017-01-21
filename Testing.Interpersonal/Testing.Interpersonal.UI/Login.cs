using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
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
namespace Testing.Interpersonal.UI
{
    [TestClass]
    public class Login : TestCase
    {
        [TestMethod]
        public void TestUserLoginAdmin()
        {
            PerformAdminLogin(this);
        }

        public static void PerformAdminLogin(TestCase tc)
        {
            try
            {
                tc.driver.Navigate().GoToUrl(baseUrl);
                tc.AssertNoJavaScriptError();

                //If logged in skip login process
                if (IsLogged(tc))
                {
                    if (IsInternalUser(tc))
                    {
                        return;
                    }
                    else
                    {
                        PerformUserLogout(tc);
                    }
                }

                tc.Click(By.XPath(@"//a[2]/span"), "Internal Sign In button");

                Assert.AreEqual("Username", tc.Find(By.XPath(@"//form[@id='login']/table/tbody/tr/td/span"), "Username label").Text);
                Assert.AreEqual("Password", tc.Find(By.XPath(@"//form[@id='login']/table/tbody/tr[2]/td/span"), "Password label").Text);

                tc.Find(By.XPath(@"//input[@name='user_email']"), "User email input").SendKeys("admin");
                tc.Find(By.XPath(@"//input[@name='user_password']"), "Password input").SendKeys("qwer1234");
                tc.Click(By.XPath(@"//input[@name='submit']"));

                Assert.AreEqual("Menu", tc.Find(By.XPath(@"//div[contains(@class,'header-bar-title')]/h2"), "Main screen label").Text);
            }
            catch (Exception ex)
            {
                LOG.logException(ex);
                throw;
            }
        }

        [TestMethod]
        public void TestUserLogout()
        {
            PerformUserLogout(this);
        }

        public static void PerformUserLogout(TestCase tc)
        {
            try
            {
                tc.AssertNoJavaScriptError();
                if (!tc.IsElementPresent(By.XPath(@"//*[contains(text(),'Logout')]/..")))
                {
                    PerformAdminLogin(tc);
                }

                tc.Click(By.XPath(@"//*[contains(text(),'Logout')]/.."), "Logout button");
                Assert.AreEqual("Internal website", tc.Find(By.XPath(@"//div[contains(@class,'header-bar-title')]/h2"), "Main screen label").Text);
            }
            catch (Exception ex)
            {
                LOG.logException(ex);
                throw;
            }
        }

        public static bool IsLogged(TestCase tc)
        {
            return tc.IsElementPresent(By.XPath(@"//*[contains(text(),'Logout')]"));
        }

        public static bool IsInternalUser(TestCase tc)
        {
            return !tc.driver.Url.Contains(@"school/external");
        }
    }
}

