using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using System.Linq;
using System.Security.Claims;


namespace QuemVemLa
{
    public class Tests
    {
        //<PackageFamilyName + !App>
        public string appID = "Microsoft.WindowsAlarms_8wekyb3d8bbwe!App";

        //Conex�o com Windows App Driver
        public string URL = @"http://127.0.0.1:4723/";

        public WindowsDriver<WindowsElement> alarm_session;       

        [SetUp]
        public void Setup()
        {
            if (alarm_session == null)
            {
                AppiumOptions opt = new AppiumOptions();
                opt.AddAdditionalCapability("platformName", "Windows");
                opt.AddAdditionalCapability("app", appID);
                opt.AddAdditionalCapability("deviceName", "WindowsPC");

                alarm_session = new WindowsDriver<WindowsElement>(new Uri(URL), opt);

                Assert.IsNotNull(alarm_session);
            }
        }

        [TearDown]
        public void ClearUP()
        {
            if (alarm_session != null) 
            {
                alarm_session.Quit();
                alarm_session = null;
            }
        }

        // Metodo 1: Verificar se as 4 principais fun��es s�o apresentadas na aplica��o: Alarm, Clock, Timer e Stopwatch. 
        [Test]
        [TestCase("AlarmButton")]
        [TestCase("TimerButton")]
        [TestCase("StopwatchButton")]
        [TestCase("ClockButton")]
        public void CheckOptions(string item)
        {
            var element = alarm_session.FindElementByAccessibilityId(item);
            bool isDisplayed = element.Displayed;
            Assert.IsTrue(isDisplayed, "The Item was found");
        }

        // Metodo 2: Adicione um alarme com hor�rio, nome, repetir, som e tocar de novo. 
        [Test]
        public void SetAlarm()
        {
            // 2. Alarm name: Sentinela
            alarm_session.FindElementByAccessibilityId("AlarmButton").Click();
            alarm_session.FindElementByAccessibilityId("AddAlarmButton").Click();
            alarm_session.FindElementByClassName("TextBox").SendKeys("Sentinela");

            // 3. Repeats: Monday, Wednesday and Friday
            // alarm_session.FindElementByAccessibilityId("RepeatCheckBox").Click();
            alarm_session.FindElementByName("Segunda-feira").Click();
            alarm_session.FindElementByName("Quarta-feira").Click();
            alarm_session.FindElementByName("Sexta-feira").Click();

            // 4. Sound: Jingle
            alarm_session.FindElementByName("Alarmes").Click();
            alarm_session.FindElementByAccessibilityId("ChimeComboBox").Click();

            // 5. Snooze time: Disabled
            alarm_session.FindElementByAccessibilityId("SnoozeComboBox").Click();
            alarm_session.FindElementByName("Desabilitado").Click();

            //6. Salve o alarm
            alarm_session.FindElementByAccessibilityId("PrimaryButton").Click();

            // Assert.AreEqual();

            // 7.Verifique se ele aparece na lista de alarmes
            alarm_session.FindElementByName("Sentinela");
            var elementAlarmList = alarm_session.FindElementByName("Sentinela");
            bool isDisplayedAlarmList = elementAlarmList.Displayed;
            Assert.IsTrue(isDisplayedAlarmList, "The new Alarm was found on the list");
        }

        // Metodo de teste 3: AddClock - Adicione um clock com a localidade "Lisbon, Portugal" e verifique se o mesmo foi adicionado
        [Test]
        [TestCase("Lisboa, Portugal", "Lisboa")]
        public void AddClock(string location, string city)
        {
            alarm_session.FindElementByAccessibilityId("ClockButton").Click();
            alarm_session.FindElementByAccessibilityId("AddClockButton").Click();

            alarm_session.FindElementByAccessibilityId("TextBox").Click();
            alarm_session.FindElementByAccessibilityId("TextBox").SendKeys(location);
            alarm_session.FindElementByAccessibilityId("TextBox").SendKeys(Keys.Enter);

            Assert.IsTrue(alarm_session.FindElementByAccessibilityId("ClockCardListView").
                FindElementsByClassName("ListViewItem")[1].GetAttribute("Name").Contains(city));
        }


        // Metodo de teste 4: TestStopwatch
        [Test]
        public void TestStopwatch()
        {
            alarm_session.FindElementByAccessibilityId("StopwatchButton").Click();
            alarm_session.FindElementByAccessibilityId("StopwatchPlayPauseButton").Click();
            System.Threading.Thread.Sleep(10000);  // PAUSA DE 10 SEGUNDOS
            alarm_session.FindElementByName("Pausa").Click();
            Assert.IsTrue(alarm_session.FindElementByAccessibilityId("StopwatchTimerText").GetAttribute("Name").Contains("10 segundos"));
        }

    }
}
