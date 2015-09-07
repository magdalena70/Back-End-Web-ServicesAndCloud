using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DependencyInversion
{
    public class ConsolePrinter
    {
        public IFormatter Formatter { get; set; }

        // constructor injection
        public ConsolePrinter(IFormatter formatter)
        {
            this.Formatter = formatter;
        }

        public void Print(string message)
        {
            var formatted = this.Formatter.Format(message);
            Console.WriteLine(formatted);
        }
    }
}
