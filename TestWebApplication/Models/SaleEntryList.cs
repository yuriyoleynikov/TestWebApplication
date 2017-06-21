using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestWebApplication.Models
{
    public class SaleEntryList
    {
        public IEnumerable<SaleEntry> SaleEntries { get; set; }
    }
}
