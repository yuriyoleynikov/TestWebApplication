using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestWebApplication.Models.ViewModels
{
    public class SalesViewModel
    {
        public IEnumerable<Sale> SaleList { get; set; }
        public IEnumerable<Product> ProductList { get; set; }
    }
}
