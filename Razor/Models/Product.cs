using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Razor.Models
{
    public class Product
    {
		public int ID { get; set; }
		public string Name { get; set; }
		public string Desc { get; set; }
		public decimal Price { get; set; }
	}
}
