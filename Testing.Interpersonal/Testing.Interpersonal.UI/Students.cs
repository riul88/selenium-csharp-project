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
    public class Students : TestCase
    {
        [TestMethod]
        public void TestGridSorting()
        {
            PerformGridSorting(this);
        }

        public static void PerformGridSorting(TestCase tc)
        {
            try
            {
                Login.PerformAdminLogin(tc);
                tc.AssertNoJavaScriptError();

                tc.Click(By.XPath(@"//*[contains(text(),'Students')]/.."), "Students button");

                //Assert.AreEqual("Username", tc.Find(By.XPath(@"//form[@id='login']/table/tbody/tr/td/span"), "Username label").Text);
                //Assert.AreEqual("Password", tc.Find(By.XPath(@"//form[@id='login']/table/tbody/tr[2]/td/span"), "Password label").Text);

                //tc.Find(By.XPath(@"//input[@name='user_email']"), "User email input").SendKeys("admin");
                //tc.Find(By.XPath(@"//input[@name='user_password']"), "Password input").SendKeys("qwer1234");
                //tc.Click(By.XPath(@"//input[@name='submit']"));

                Assert.AreEqual("List of students", tc.Find(By.XPath(@"//div[contains(@class,'header-title')]/h2"), "Menu").Text);

                Login.PerformUserLogout(tc);
            }
            catch (Exception ex)
            {
                LOG.logException(ex);
                throw;
            }
        }
    }
}
