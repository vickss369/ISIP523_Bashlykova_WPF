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

        public static double GetPriceWithDiscount(Product product)
        {
            if (product.Discount != null && product.Discount > 0)
            {
                return product.Price * (1 - product.Discount.Value / 100.0);
            }

            return product.Price;
        }

        public void RemoveProduct(Product product)
        {
            if (ProductsInBasket.Contains(product))
                ProductsInBasket.Remove(product);
        }
    }
}
