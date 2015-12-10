using System;
using Microsoft.Practices.Unity;

namespace Mailfunnel
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            try
            {

            
            var container = Bootstrapper.InitialiseContainer();

            var initialiser = container.Resolve<IInitialiser>();
            initialiser.Initialise();

            Console.ReadLine();
            }
            catch (Exception ex)
            {

            }
        }
    }
}