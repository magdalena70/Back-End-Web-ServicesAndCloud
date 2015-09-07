using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsingHttpClient
{
    public class AdsDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public object Owner { get; set; }
        public string Type { get; set; }
        public DateTime PostedOn { get; set; }
        public object Categories { get; set; }
    }
}
