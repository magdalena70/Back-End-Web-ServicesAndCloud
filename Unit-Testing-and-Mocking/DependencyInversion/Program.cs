using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DependencyInversion
{
    class Program
    {
        static void Main(string[] args)
        {
            // Ninject
            var kernel = new StandardKernel();
            Bindings.RegisterBindings(kernel);

            var jsonFormatter = new JsonFormatter();
            var xmlFormatter = kernel.Get<IFormatter>();
            var printer = new ConsolePrinter(xmlFormatter);
            printer.Print("Hello, message!");
        }
    }
}
