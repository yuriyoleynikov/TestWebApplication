using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TestWebApplication.Services;
using TestWebApplication.Models;
using TestWebApplication.Models.ViewModels;

namespace TestWebApplication.Controllers
{
    public class HomeController : Controller
    {
        private IRepositoryService _repositoryService;

        public HomeController(IRepositoryService repositoryService)
        {
            _repositoryService = repositoryService;
        }

        public static decimal GetSurrender(decimal cash, decimal total)
        {
            if (cash < total)
                throw new ArgumentException("cash < total");

            return cash - total;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Sales()
        {
            var salesViewModel = new SalesViewModel();
            salesViewModel.SaleList = _repositoryService.GetSaleList();
            salesViewModel.ProductList = _repositoryService.GetProductList();

            return View(salesViewModel);
        }

        public IActionResult Sale(IEnumerable<int> productId = null, IEnumerable<decimal> quantities = null)
        {
            var saleViewModel = new SaleViewModel();

            var tempSaleEntryModel = new List<SaleEntryModel>();

            using (var prodId = productId.GetEnumerator())
            using (var quants = quantities.GetEnumerator())
            {
                while (prodId.MoveNext() && quants.MoveNext())
                    tempSaleEntryModel.Add(new SaleEntryModel { ProductId = prodId.Current, Quantity = quants.Current });
            }
            saleViewModel.SaleEntryModel = tempSaleEntryModel;
            saleViewModel.ProductList = _repositoryService.GetProductList();

            return View(saleViewModel);
        }

        [HttpPost]
        public IActionResult AddSaleEntry(string name, decimal quantity, IEnumerable<int> productId = null, IEnumerable<decimal> quantities = null)
        {
            try
            {
                var currentProductId = _repositoryService.GetProductIdByName(name);

                if (productId == null)
                    productId = new List<int> { currentProductId };
                else
                {
                    var tempProductId = productId.ToList();
                    tempProductId.Add(currentProductId);
                    productId = tempProductId;
                }

                if (quantities == null)
                    quantities = new List<decimal> { quantity };
                else
                {
                    var tempQuantities = quantities.ToList();
                    tempQuantities.Add(quantity);
                    quantities = tempQuantities;
                }

                return RedirectToAction(nameof(Sale), new { productId, quantities });
            }
            catch (KeyNotFoundException)
            {
                return RedirectToAction(nameof(Sale), new { productId, quantities });
            }
        }

        [HttpPost]
        public IActionResult Sell(decimal cash, IEnumerable<int> productId, IEnumerable<decimal> quantities)
        {
            List<SaleEntry> saleEntry = new List<SaleEntry>();
            using (var prodId = productId.GetEnumerator())
            using (var quants = quantities.GetEnumerator())
            {
                while (prodId.MoveNext() && quants.MoveNext())
                    saleEntry.Add(new SaleEntry
                    {
                        ProductId = prodId.Current,
                        Quantity = quants.Current,
                        Price = _repositoryService.GetPriceByProductId(prodId.Current)
                    });
            }

            var sale = new Sale();
            sale.SaleEntries = saleEntry;

            var saleId = _repositoryService.Sell(sale);

            return RedirectToAction(nameof(Check), new { saleId, cash });
        }

        public IActionResult Check(int saleId, decimal cash)
        {
            var checkViewModel = new CheckViewModel();
            checkViewModel.Sale = _repositoryService.GetSaleBySaleId(saleId);
            checkViewModel.Cash = cash;
            checkViewModel.ProductList = _repositoryService.GetProductList();

            return View(checkViewModel);
        }

        public IActionResult Stock()
        {
            var productList = _repositoryService.GetProductList();

            return View(productList);
        }

        [HttpGet]
        public IActionResult EditProduct(int productId)
        {
            var products = _repositoryService.GetProductList();
            var editProduct = products.SingleOrDefault(product => product.ProductId == productId);

            return View(editProduct);
        }

        [HttpGet]
        public IActionResult DetailsProduct(int productId)
        {
            var products = _repositoryService.GetProductList();
            var detailsProduct = products.SingleOrDefault(product => product.ProductId == productId);
            var productViewModel = new ProductViewModel
            {
                Description = detailsProduct.Description,
                Name = detailsProduct.Name,
                Price = detailsProduct.Price,
                ProductCode = detailsProduct.ProductCode,
                ProductId = detailsProduct.ProductId,
                Quantity = detailsProduct.Quantity
            };

            return View(productViewModel);
        }

        [HttpPost]
        public IActionResult EditProduct(int id, Product product)
        {
            product.ProductId = id;

            try
            {
                _repositoryService.Update(product);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Stock));
        }
        
        public IActionResult DeleteProduct(int productId)
        {
            try
            {
                _repositoryService.DeleteProduct(productId);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            
            return RedirectToAction(nameof(Stock));
        }
        
        [HttpGet]
        public IActionResult AddProduct()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddProduct(Product product)
        {
            try
            {
                _repositoryService.AddProduct(product);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Stock));
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
