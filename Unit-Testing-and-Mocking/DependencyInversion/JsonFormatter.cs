using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DependencyInversion
{
    public class JsonFormatter : IFormatter
    {
        public string Format(string message)
        {
            return "{ \"message\": \"" + message + "\" }";
        }
    }
}
