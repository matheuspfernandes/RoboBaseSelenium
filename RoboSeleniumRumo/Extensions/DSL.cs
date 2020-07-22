using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Threading;

namespace RoboSeleniumRumo.Extensions
{
    public static class DSL
    {
        public static void LimpaCampoEEscreve(this IWebElement Elemento, string Texto)
        {
            Elemento.Clear();
            Elemento.SendKeys(Texto);
            Console.WriteLine("Limpou campo e inseriu o texto \"" + Texto + "\" no campo");
        }

        public static void RealizaClique(this IWebElement Elemento)
        {
            string ElementoClicadoId = " " + Elemento.GetAttribute("id") != null ? Elemento.GetAttribute("id") : "";
            string ElementoClicadoLocation = " " + Elemento.Location != null ? Elemento.Location.ToString() : "";
            Elemento.Click();

            Console.WriteLine("Realizou clique em " + ElementoClicadoId + "  " + ElementoClicadoLocation);
        }

        public static void SelecionaDropDownPeloTexto(this IWebElement Elemento, string Texto)
        {
            new SelectElement(Elemento).SelectByText(Texto);
            Console.WriteLine("Selecionou a opção \"" + Texto + "\" no dropdown");
        }

        public static void SelecionaDropDownPeloValue(this IWebElement Elemento, string Texto)
        {
            new SelectElement(Elemento).SelectByValue(Texto);
            Console.WriteLine("Selecionou a opção com value \"" + Texto + "\" no dropdown");
        }

        public static void LimpaCampo(this IWebElement Elemento)
        {
            Elemento.Clear();
            Console.WriteLine("Limpou o campo");
        }

        public static void EscreveNoCampo(this IWebElement Elemento, string Texto)
        {
            Elemento.SendKeys(Texto);
            Console.WriteLine("Inseriu o texto \"" + Texto + "\" no campo");
        }

        public static void Tab(this IWebElement Elemento)
        {
            Elemento.SendKeys(Keys.Tab);
            Console.WriteLine("Deu tab no campo");
            Thread.Sleep(1000);
        }

        public static void Enter(this IWebElement Elemento)
        {
            Elemento.SendKeys(Keys.Enter);
            Console.WriteLine("Deu Enter no campo");
            Thread.Sleep(1000);
        }
    }
}
