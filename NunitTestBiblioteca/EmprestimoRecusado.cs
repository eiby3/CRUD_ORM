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
    public class EmprestimoRecusado
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
        public void NaoDevePermitirCriacaoDeEmprestimo()
        {
            driver.Navigate().GoToUrl("https://localhost:44306/EmprestimoModels/Create");
            driver.FindElement(By.Id("LivroId")).Click();
            driver.FindElement(By.Id("ClienteId")).Click();
            new SelectElement(driver.FindElement(By.Id("ClienteId"))).SelectByText("Abe");
            driver.FindElement(By.Id("Emprestado")).Click();
            driver.FindElement(By.Id("PrevisaoDevolucao")).Click();
            driver.FindElement(By.Id("verificarEmprestimo")).Click();
            driver.Navigate().GoToUrl("https://localhost:44306/EmprestimoModels/Devolver/0?LivroId=1&ClienteId=2&Emprestado=02%2F16%2F2022%2000%3A00%3A00&PrevisaoDevolucao=02%2F21%2F2022%2000%3A00%3A00");


            //Assert.NotNull(driver.FindElement(By.XPath("//h1[contains(text(), 'CLIENTE POSSUI EMPRESTIMO ABERTO')]")));
            Assert.NotNull(driver.FindElement(By.XPath("//h1[@class='emprestimoAberto']")));
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
