using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestWebApplication.Models.ViewModels
{
    public class SaleViewModel
    {
        public IEnumerable<Product> ProductList { get; set; }
        public IEnumerable<SaleEntryModel> SaleEntryModel { get; set; }
    }
}
