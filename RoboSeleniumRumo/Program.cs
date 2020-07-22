using RoboSeleniumRumo.WebCrawler.Acesso;

namespace RoboSeleniumRumo
{
    class Program
    {
        static void Main(string[] args)
        {
            GooglePage gp = new GooglePage();
            gp.UIAcessarGoogle();
        }
    }
}
