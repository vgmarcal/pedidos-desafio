using System;

namespace Pedidos.Domain.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }

        protected Product()
        {
        }

        public Product(string productName, decimal productPrice)
        {
            if (string.IsNullOrWhiteSpace(productName))
                throw new ArgumentException("O nome do produto é obrigatório.", "productName");

            if (productPrice < 0)
                throw new ArgumentException("O valor do produto não pode ser negativo.", "productPrice");

            Name = productName.Trim();
            Price = productPrice;
        }
    }
}
