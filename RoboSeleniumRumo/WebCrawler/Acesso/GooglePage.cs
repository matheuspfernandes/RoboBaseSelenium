using OpenQA.Selenium;
using RoboSeleniumRumo.Extensions;
using RoboSeleniumRumo.WebCrawler.Base;
using System;

namespace RoboSeleniumRumo.WebCrawler.Acesso
{
    public class GooglePage : DriverFactory
    {
        public void UIAcessarGoogle()
        {
            try
            {
                InicializaBrowserAnonimo("https://www.google.com.br/");

                driver.FindElement(By.Name("q")).LimpaCampoEEscreve("Rumo Soluções");
                driver.FindElement(By.Name("q")).Enter();
                driver.FindElement(By.LinkText("Imagens")).RealizaClique();
                TirarPrint();

                driver.FindElement(By.LinkText("Todas")).RealizaClique();
                EsperaExplicitaEmMilisegundos(2500);
                driver.FindElement(By.XPath("//*[@id='rso']//h3")).RealizaClique();
                EsperaExplicitaEmMilisegundos(2500);
                TirarPrint();

                Console.WriteLine("Acessou pagina do Google e tirou print");
            }
            catch (Exception e)
            {
                TirarPrint();

                throw e;
            }
            finally
            {
                FinalizaNavegador();
            }

        }

    }
}
