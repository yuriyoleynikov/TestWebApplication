using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestWebApplication.Models.ViewModels
{
    public class CheckViewModel
    {
        public Sale Sale { get; set; }
        public IEnumerable<Product> ProductList { get; set; }
        public decimal Cash { get; set; }
    }
}
