using Grocery.Core.Interfaces.Repositories;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;

namespace Grocery.Core.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public List<Product> GetAll()
        {
            return _productRepository.GetAll();
        }

        public Product Add(Product item)
        {
            if (string.IsNullOrWhiteSpace(item.Name))
            {
                throw new ArgumentException("Productnaam mag niet leeg zijn.");
            }

            if (item.Stock < 0)
            {
                throw new ArgumentException("Voorraad kan niet negatief zijn.");
            }

            if (item.Price < 0)
            {
                throw new ArgumentException("Prijs kan niet negatief zijn.");
            }

            if (item.ShelfLife < DateOnly.FromDateTime(DateTime.Now))
            {
                throw new ArgumentException("THT datum mag niet in het verleden liggen.");
            }

            // Product toevoegen via repository
            return _productRepository.Add(item);
        }

        public Product? Delete(Product item)
        {
            return _productRepository.Delete(item);
        }

        public Product? Get(int id)
        {
            return _productRepository.Get(id);
        }

        public Product? Update(Product item)
        {
            return _productRepository.Update(item);
        }
    }
}