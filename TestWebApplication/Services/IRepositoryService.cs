using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestWebApplication.Models;

namespace TestWebApplication.Services
{
    public interface IRepositoryService
    {
        IEnumerable<Product> GetProductList();
        IEnumerable<Sale> GetSaleList();
        int Sell(Sale sale);
        int GetProductIdByName(string name);
        decimal GetPriceByProductId(int productId);
        Sale GetSaleBySaleId(int saleId);
        void AddProduct(Product product);
        void Update(Product product);
        void DeleteProduct(int id);
    }
}
