using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;

namespace MvcSuperShop.UItests
{
    [TestClass]
    public class UITests
    {
        private static IWebDriver _driver;

        [ClassInitialize]
        public static void Init(TestContext Context)
        {
            _driver = new ChromeDriver();
        }

        [ClassCleanup]
        public static void Cleanup()
        {
            _driver.Close();
            _driver.Dispose();
        }

        [TestMethod]
        public void When_registering_should_return_correct_url()
        {
            _driver.Navigate().GoToUrl("https://localhost:7122/");

            Wait(3);

            var allButtons = _driver.FindElements(By.LinkText("Register"));

            var btn = allButtons.First();

            btn.Click();

            Wait(3);
            var emailGuid = Guid.NewGuid();
            var registerEmail = $"{emailGuid}@gmail.com";
            var password = "Hejsan123!";

            var emailInput = _driver.FindElement(By.Id("Input_Email"));
            emailInput.SendKeys(registerEmail.ToString());

            var passwordInput = _driver.FindElement(By.Id("Input_Password"));
            passwordInput.SendKeys(password);

            var passwordConfirmInput = _driver.FindElement(By.Id("Input_ConfirmPassword"));
            passwordConfirmInput.SendKeys(password);

            Wait(3);

            var register = _driver.FindElement(By.Id("registerSubmit"));

            register.Click();

            Assert.AreEqual($"https://localhost:7122/Identity/Account/RegisterConfirmation?email={registerEmail}&returnUrl=%2F", _driver.Url);
        }

        [TestMethod]
        public void When_logging_in_return_correct_url()
        {
            _driver.Navigate().GoToUrl("https://localhost:7122/");

            var allButtons = _driver.FindElements(By.LinkText("Login"));

            var btn = allButtons.First();

            btn.Click();

            Wait(3);

            var loginEmail = "stefan.holmberg@systementor.se";
            var password = "Hejsan123#";

            var emailInput = _driver.FindElement(By.Id("Input_Email"));
            emailInput.SendKeys(loginEmail);
            Wait(2);

            var passwordInput = _driver.FindElement(By.Id("Input_Password"));
            passwordInput.SendKeys(password);

            Wait(2);

            var login = _driver.FindElement(By.Id("login-submit"));

            login.Click();

            Wait(1);

            Assert.AreEqual("https://localhost:7122/", _driver.Url);
        }
        
        private void Wait(int sec = 1)
        {
            Thread.Sleep(sec * 3000);
        }

        
    }
}