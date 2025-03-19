using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using System.Linq;


namespace QuemVemLa
{
    public class Tests
    {
        // armazenar o valor do appID numa variavel temporaria
        // <PackageFamilyName + !App>
        // concatenar esses dois. "!App" pra identificar como aplicativo
        public string appID = "Microsoft.WindowsCalculator_8wekyb3d8bbwe!App";

        //Conexao com Windows App Driver
        public string URL = @"http://127.0.0.1:4723/";

        public WindowsDriver<WindowsElement> calc_session;       

        [SetUp]
        public void Setup()
        {
            // Aqui vai tudo que deve se fazer ANTES de executar um teste
            // abra o app; verifique se abriu...

            if (calc_session == null)
            {
                AppiumOptions opt = new AppiumOptions();
                opt.AddAdditionalCapability("platformName", "Windows");
                opt.AddAdditionalCapability("app", appID);
                opt.AddAdditionalCapability("deviceName", "WindowsPC");

                calc_session = new WindowsDriver<WindowsElement>(new Uri(URL), opt);
                // uma sessao para cada app caso seu teste mexa com mais de uma aplicacao

                Assert.IsNotNull(calc_session);
            }
        }

        // Tag TierDown pra limpar a aplicacao pra nao impactar o proximo teste tambem
        [TearDown]
        public void ClearUP()
        {
            if (calc_session != null) 
            {
                calc_session.Quit();
                calc_session = null;
            }
        }

        [Test]
        // Com o atributo TestCase vc coloca o cenario de teste pra nao colocar na mao
        [TestCase("num8Button", "plusButton", "num2Button", "equalButton", "10")]
        [TestCase("num9Button", "plusButton", "num2Button", "equalButton", "11")]
        [TestCase("num9Button", "plusButton", "num9Button", "equalButton", "18")] 

        public void Soma(string num1, string operacao, string num2, string igual, string result)
        {
            // Pega o Automation ID no inspect
            // qual sessao .o que vou usar . acao/retorno/etc - ver na doc apium github
                    // NA MAO: calc_session.FindElementByAccessibilityId("num8Button").Click();
            calc_session.FindElementByAccessibilityId(num1).Click();
            calc_session.FindElementByAccessibilityId(operacao).Click();
            calc_session.FindElementByAccessibilityId(num2).Click();
            calc_session.FindElementByAccessibilityId(igual).Click();

            // Tratamento para pegar so o numero na string. Se o win tiver em ingles a string muda.
            var calc_result = calc_session.FindElementByAccessibilityId("CalculatorResults")
                .Text.Replace("A exibicao eh ", string.Empty).Trim();

            //esperado, atual, mensagem
            Assert.AreEqual(result, calc_result, "O resultado nao eh igual ao esperado.");

        }


        [Test]
        [TestCase("num8Button", "multiplyButton", "num2Button", "equalButton", "16")]
        [TestCase("num5Button", "multiplyButton", "num5Button", "equalButton", "25")]
        public void Multiplicacao(string num1, string operacao, string num2, string igual, string result)
        {
            calc_session.FindElementByAccessibilityId(num1).Click();
            calc_session.FindElementByAccessibilityId(operacao).Click();
            calc_session.FindElementByAccessibilityId(num2).Click();
            calc_session.FindElementByAccessibilityId(igual).Click();

            var calc_result = calc_session.FindElementByAccessibilityId("CalculatorResults").Text
                .Replace("A exibicao eh ", string.Empty).Trim();

            Assert.AreEqual(result, calc_result, "O resultado nao eh igual ao esperado.");
        }


        [Test]
        public void Divisao()
        {
            Assert.Pass();
        }


    }
}