using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DependencyInversion
{
    public class Bindings
    {
        public static void RegisterBindings(IKernel kernel)
        {
            kernel.Bind<IFormatter>().To<XmlFormatter>();
        }
    }
}
