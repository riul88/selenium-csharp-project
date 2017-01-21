using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testing.Common;

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

                Assert.AreEqual("List of students", tc.Find(By.XPath(@"//div[contains(@class,'header-bar-title')]/h2"), "Menu").Text);

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
