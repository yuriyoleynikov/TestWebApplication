using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestWebApplication.Data;
using TestWebApplication.Models;

namespace TestWebApplication.Services
{
    public class DatabaseRepositoryService : IRepositoryService
    {
        private readonly ShopDbContext _context;

        public DatabaseRepositoryService(ShopDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Models.Product> GetProductList()
        {
            var result = _context.Products
                .Select(x =>
                    new Models.Product
                    {
                        ProductId = x.ProductId,
                        Name = x.Name,
                        Description = x.Description,
                        ProductCode = x.ProductCode,
                        Quantity = x.Quantity,
                        Price = x.Price
                    }).ToList();
            return result;
        }

        public IEnumerable<Models.Sale> GetSaleList()
        {
            var result = _context.Sales
                .Select(sale =>
                    new Models.Sale
                    {
                        SaleId = sale.SaleId,
                        Date = sale.Date,
                        SaleEntries = sale.SaleEntries
                            .Select(saleEntry =>
                                new Models.SaleEntry
                                {
                                    Price = saleEntry.Price,
                                    ProductId = saleEntry.ProductId,
                                    Quantity = saleEntry.Quantity
                                }).ToArray()
                    });
            return result;
        }

        public int GetProductIdByName(string name)
        {
            var result = _context.Products;
            if (!result.Select(x => x.Name).Contains(name))
                throw new KeyNotFoundException("Product was not found.");
            return result.SingleOrDefault(product => product.Name == name).ProductId;
        }

        public void Update(Models.Product product)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));

            var productUpdate = _context.Products.SingleOrDefault(prod => prod.ProductId == product.ProductId);

            if (productUpdate == null)
                throw new KeyNotFoundException("Product was not found.");

            productUpdate.Name = product.Name;
            productUpdate.Description = product.Description;
            productUpdate.Price = product.Price;
            productUpdate.ProductCode = product.ProductCode;
            productUpdate.Quantity = product.Quantity;

            _context.SaveChanges();
        }

        public int Sell(Models.Sale sale)
        {
            foreach (var entry in sale.SaleEntries)
            {
                var productSell = _context.Products.SingleOrDefault(prod => prod.ProductId == entry.ProductId);
                productSell.Quantity -= entry.Quantity;
            }

            var SaleList = _context.Sales;
            SaleList.Add(new Data.Sale
            {
                Date = DateTime.Now,
                SaleEntries = sale.SaleEntries
                    .Select(entry => new Data.SaleEntry { ProductId = entry.ProductId, Quantity = entry.Quantity, Price = entry.Price }).ToList()
            });

            _context.SaveChanges();
            var currentSale = SaleList.LastOrDefault();

            return currentSale.SaleId;
        }

        public decimal GetPriceByProductId(int productId)
        {
            var result = _context.Products;
            if (!result.Select(x => x.ProductId).Contains(productId))
                throw new KeyNotFoundException("ProductId was not found.");
            return result.SingleOrDefault(product => product.ProductId == productId).Price;
        }

        public Models.Sale GetSaleBySaleId(int saleId)
        {
            var result = _context.Sales.Select(dResult => new Models.Sale
            {
                SaleId = dResult.SaleId,
                Date = dResult.Date,
                SaleEntries = dResult.SaleEntries.Select(saleEntry =>
                    new Models.SaleEntry
                    {
                        Price = saleEntry.Price,
                        ProductId = saleEntry.ProductId,
                        Quantity = saleEntry.Quantity
                    })
            }).SingleOrDefault(s => s.SaleId == saleId);


            return result;
        }

        public void DeleteProduct(int idproductId)
        {
            var product = _context.Products.SingleOrDefault(prod => prod.ProductId == idproductId);

            if (product == null)
                throw new KeyNotFoundException("Product was not found.");

            _context.Products.Remove(product);
            _context.SaveChanges();
        }

        public void AddProduct(Models.Product product)
        {
            if (product == null)
                throw new KeyNotFoundException("Product was not found.");

            _context.Products.Add(new Data.Product
            {
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                ProductCode = product.ProductCode,
                Quantity = product.Quantity
            });
            _context.SaveChanges();
        }
    }
}