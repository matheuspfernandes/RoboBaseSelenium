using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Threading;

namespace RoboSeleniumRumo.WebCrawler.Base
{
    public abstract class DriverFactory
    {
        public SqlConnectionStringBuilder SqlSB = new SqlConnectionStringBuilder();
        public ChromeOptions options;

        protected IWebDriver driver;
        protected WebDriverWait wait;

        public DriverFactory()
        {

        }


        #region [JS]

        public object ExecutaComandoJavaScript(string ComandoJS)
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            return js.ExecuteScript(ComandoJS);
        }

        public void RealizaScrollAteElemento(By by)
        {
            IWebElement element = driver.FindElement(by);
            ExecutaComandoJavaScript("window.scrollBy(0, " + element.Location.Y + ")");
        }

        //public void MarcaPosicaoOndeDeveriaReceberOClique(int X, int Y)
        //{
        //    ExecutaComandoJavaScript("document.elementFromPoint(" + X + "," + Y + ").style.color = 'red'");
        //}

        #endregion


        #region [Esperas (Sincronismo)]

        public void EsperaPorElementoClicavel(By by)
        {
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
            wait.Until(ExpectedConditions.ElementToBeClickable(by));
        }

        public void EsperaPorElementoVisivel(By by)
        {
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
            wait.Until(ExpectedConditions.ElementIsVisible(by));
        }

        public void EsperaPorElementosLocalizadosPor(By elemento, int tempo)
        {
            new WebDriverWait(driver, TimeSpan.FromSeconds(tempo)).Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(elemento));
        }

        public void EsperaAteMudancaDoAtributoDoElemento(By by, string AtributoDesejado, string NovoValor)
        {
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(45));

            wait.Until(driver => driver.FindElement(by).Enabled
                  && RetornaValorDoAtributoDeUmElemento(driver.FindElement(by), AtributoDesejado).Contains(NovoValor)
              );
        }

        public void EsperaExplicitaEmMilisegundos(double TempoEmMilisegundos)
        {
            Thread.Sleep(Convert.ToInt32(TempoEmMilisegundos));
            Console.WriteLine("Esperou " + TempoEmMilisegundos / 1000 + " segundos");
        }

        #endregion


        #region[Esta presente na tela?]

        public bool IsElementPresent(By by)
        {
            try
            {
                driver.FindElement(by);
                return false;
            }
            catch
            {
                return false;
            }
        }

        public bool isAlertPresent()
        {
            try
            {
                driver.SwitchTo().Alert();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool AreElementsPresent(By by)
        {
            try
            {
                ReadOnlyCollection<IWebElement> ElementsList = driver.FindElements(by);

                if (ElementsList.Count == 0)
                {
                    Console.WriteLine("Não foi encontrado nenhum elemento pelo locator: " + by.ToString());
                    return false;
                }
                else
                {
                    foreach (IWebElement element in ElementsList)
                    {
                        if (!element.Displayed)
                        {
                            Console.WriteLine("Um dos elementos não foram encontrados pelo locator " + by.ToString());
                            return false;
                        }

                    }

                    return true;
                }
            }

            catch (NoSuchElementException)
            {
                Console.WriteLine("Não foi encontrado nenhum elemento pelo locator: " + by.ToString());
                return false;
            }
        }

        public bool OpcaoEstaClicavelNoDropDown(By LocatorDoDropDown, string NomeDaOpcaoDesejada)
        {
            EsperaPorElementoClicavel(LocatorDoDropDown);
            driver.FindElement(LocatorDoDropDown).Click();
            var DropDown = new SelectElement(driver.FindElement(LocatorDoDropDown)).Options;

            foreach (IWebElement ItemDropDown in DropDown)
            {

                if (ItemDropDown.Text.Equals(NomeDaOpcaoDesejada))
                {
                    if ((RetornaValorDoAtributoDeUmElemento(ItemDropDown, "disabled")).Equals("empty") &&      //Se ele não tiver o disabled
                       (RetornaValorDoAtributoDeUmElemento(ItemDropDown, "style")).Equals("empty"))            //Nem tiver o style
                    {
                        Console.WriteLine("Opção " + NomeDaOpcaoDesejada + " está presente e clicavel no dropdown");
                        return true;
                    }

                    else if (!(RetornaValorDoAtributoDeUmElemento(ItemDropDown, "disabled").Equals("true")) &&              //E não tiver o disabled ativado
                            (!(RetornaValorDoAtributoDeUmElemento(ItemDropDown, "style").Equals("display: none;"))))        //E não estiver com display desativado
                    {
                        Console.WriteLine("Opção " + NomeDaOpcaoDesejada + " está presente e clicavel no dropdown");
                        return true;
                    }

                }

            }

            Console.WriteLine("Opção " + NomeDaOpcaoDesejada + " não está presente e clicavel no dropdown");
            return false;

        }


        #endregion


        #region [Captura de tela]

        public void TirarPrint()
        {
            DateTime dateTime = DateTime.Now;
            string horaAtual = dateTime.ToString();
            horaAtual = horaAtual.Replace('/', '-'); horaAtual = horaAtual.Replace(':', '_');

            string pastaArquivo = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            try
            {
                Screenshot ss = ((ITakesScreenshot)driver).GetScreenshot();
                ss.SaveAsFile(pastaArquivo + "\\fullScreenShot " + horaAtual + ".jpeg", ScreenshotImageFormat.Jpeg);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw e;
            }

        }

        #endregion


        #region [Metodos do browser]

        protected void AcessaLink(string link)
        {
            driver.Navigate().GoToUrl(link);
        }

        protected void InicializaBrowserAnonimo(string linkToAccess)
        {
            options = new ChromeOptions();
            options.AddArguments("--incognito");

            InicializaBrowser(linkToAccess);
        }

        protected void InicializaBrowserAnonimoHeadless(string linkToAccess)
        {
            options = new ChromeOptions();
            options.AddArgument("--headless");
            options.AddArguments("--incognito");

            InicializaBrowser(linkToAccess);
        }

        //OS PRINTS FICAM BUGADOS
        protected void InicializaBrowserHeadLess(string linkToAccess)
        {
            options = new ChromeOptions();
            options.AddArgument("--headless");

            InicializaBrowser(linkToAccess);
        }

        protected void InicializaBrowser(string linkToAccess)
        {
            options.AddArgument("--start-maximized");

            driver = new ChromeDriver(options);
            AcessaLink(linkToAccess);
        }

        protected void FinalizaNavegador()
        {
            try
            {
                driver.Quit();
                driver.Dispose();
            }
            catch (Exception)
            {
                Console.WriteLine("Caiu na exception");
            }
        }

        #endregion


        #region [Retorno]

        /// <summary>
        /// Retorna o texto contido no elemento. Caso não tiver nenhum texto, retorna "empty"
        /// </summary>
        /// <param name="element"></param>
        /// <param name="AtributoDesejado"></param>
        /// <returns></returns>
        public string RetornaValorDoAtributoDeUmElemento(IWebElement element, string AtributoDesejado)
        {
            try
            {
                if (String.IsNullOrEmpty(element.GetAttribute(AtributoDesejado)))
                    return "empty";
                else
                    return element.GetAttribute(AtributoDesejado);
            }

            catch (Exception)
            {
                return "empty";
            }

        }

        #endregion

    }
}
