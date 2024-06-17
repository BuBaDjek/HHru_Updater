using System;
using System.Reflection.Metadata.Ecma335;
using OpenQA;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.DevTools.V123.Debugger;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Interactions.Internal;
using OpenQA.Selenium.Support.UI;



namespace FirstProgram
{
    class DemoPrint
    {
        public static string[] LogPas()
        {
            try
            {
                string[] logPas = new string[2];
                StreamReader lg = new StreamReader("C:\\Login.txt");
                StreamReader pw = new StreamReader("C:\\Password.txt");
                logPas[0] = lg.ReadLine();
                logPas[1] = pw.ReadLine();
                lg.Close();
                pw.Close();

                return logPas;
            }
            catch (Exception e)
            {
                Console.WriteLine("Ошибка: " + e.Message);
            }
            finally
            {
                Console.WriteLine("Чтение файла завершено");
            }
            return ["0", "0"];
        }
        static void Main()
        {
            var chromeOptions = new ChromeOptions()
            {
                BinaryLocation = @"C:\Program Files\Google\Chrome\Application\chrome.exe"

            };
            chromeOptions.PageLoadStrategy = PageLoadStrategy.None;
            chromeOptions.AddArgument("--no-sandbox");
            chromeOptions.AddArgument("--disable-extensions");
            chromeOptions.AddArgument("--disable-plugins");
            chromeOptions.AddArgument("--guets");
            chromeOptions.AddArgument("--disable-images");
            chromeOptions.AddArgument("--disable-logging");
            chromeOptions.AddArgument("--disable-blink-features=AutomationControlled");


            var driver = new ChromeDriver(chromeOptions);
            Actions action = new Actions(driver);

            driver.Manage().Timeouts().PageLoad.Add(TimeSpan.FromSeconds(120));
            driver.Navigate().GoToUrl("https://kazan.hh.ru/account/login?backurl=%2F&hhtmFrom=main");
            Thread.Sleep(3000);
            action.MoveToElement(driver.FindElement(By.XPath("//*[@data-qa=\"expand-login-by-password\"]"))).Click().Perform();
            Thread.Sleep(3000);
            driver.FindElement(By.XPath("//*[@data-qa=\"login-input-username\"]")).SendKeys(LogPas()[0]);
            driver.FindElement(By.XPath("//*[@data-qa=\"login-input-password\"]")).SendKeys(LogPas()[1]);
            driver.FindElement(By.XPath("//*[@data-qa=\"account-login-submit\"]")).Click();
            Thread.Sleep(3000);
            driver.FindElement(By.XPath("//*[@data-qa=\"mainmenu_myResumes\"]")).Click();

            var element = driver.FindElements(By.XPath("//*[@data-qa=\"resume-update-button_actions\"]"));
            Thread.Sleep(5000);
            action.MoveToElement(driver.FindElement(By.XPath("//*[@data-qa=\"cookies-policy-informer-accept\"]"))).Click().Perform();

            foreach (var l in element)
            {
                try
                {
                    Thread.Sleep(1000);
                    if (l.Text == "Поднять в поиске")
                    {
                        action.MoveToElement(l).Click().Build().Perform();
                        Thread.Sleep(2000);
                        action.MoveToElement(driver.FindElement(By.XPath("//*[@data-qa=\"bot-update-resume-modal__close-button\"]"))).Click().Perform();
                        Console.WriteLine(l);
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Резюме поднято");
                }
            }

            Console.WriteLine("Конец");
            driver.Close();
        }
    }
}