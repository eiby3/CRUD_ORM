using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;

namespace SeleniumTests
{
    [TestFixture]
    public class UntitledTestCase
    {
        private IWebDriver driver;
        private StringBuilder verificationErrors;
        private bool acceptNextAlert = true;

        [SetUp]
        public void SetupTest()
        {
            var option = new ChromeOptions()
            {
                BinaryLocation = @"C:\Program Files\Google\Chrome\Application\chrome.exe"
            };
            driver = new ChromeDriver(option);
            verificationErrors = new StringBuilder();
        }

        [TearDown]
        public void TeardownTest()
        {
            try
            {
                driver.Quit();
            }
            catch (Exception)
            {
                // Ignore errors if unable to close the browser
            }
            Assert.AreEqual("", verificationErrors.ToString());
        }

        [Test]
        public void DevePermitirEmprestimo()
        {
            driver.Navigate().GoToUrl("https://localhost:44306/EmprestimoModels/Create");
            driver.FindElement(By.Id("LivroId")).Click();
            new SelectElement(driver.FindElement(By.Id("LivroId"))).SelectByText("Percy Jackson");
            driver.FindElement(By.Id("ClienteId")).Click();
            new SelectElement(driver.FindElement(By.Id("ClienteId"))).SelectByText("Jorginho");
            driver.FindElement(By.Id("Emprestado")).Click();
            driver.FindElement(By.Id("PrevisaoDevolucao")).Click();
            driver.FindElement(By.Id("verificarEmprestimo")).Click();
            driver.Navigate().GoToUrl("https://localhost:44306/EmprestimoModels");

            Assert.NotNull(driver.FindElement(By.XPath("//h1[@class='listaDeEmprestimos']")));

        }
        private bool IsElementPresent(By by)
        {
            try
            {
                driver.FindElement(by);
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        private bool IsAlertPresent()
        {
            try
            {
                driver.SwitchTo().Alert();
                return true;
            }
            catch (NoAlertPresentException)
            {
                return false;
            }
        }

        private string CloseAlertAndGetItsText()
        {
            try
            {
                IAlert alert = driver.SwitchTo().Alert();
                string alertText = alert.Text;
                if (acceptNextAlert)
                {
                    alert.Accept();
                }
                else
                {
                    alert.Dismiss();
                }
                return alertText;
            }
            finally
            {
                acceptNextAlert = true;
            }
        }
    }
}
