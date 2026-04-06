using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PR12.Classes
{
    public class Basket
    {
        public static Basket currentBasket { get; set; } = new Basket();

        public List<Product> ProductsInBasket { get; set; } = new List<Product>();

        public double TotalPrice => ProductsInBasket.Sum(p => p.Price);

        public bool AddProduct(Product product)
        {
            if (product.IsFreeze)
                return false;

            ProductsInBasket.Add(product);
            return true;
        }

        public void RemoveProduct(Product product)
        {
            if (ProductsInBasket.Contains(product))
                ProductsInBasket.Remove(product);
        }
    }
}
