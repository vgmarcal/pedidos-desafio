using System;

namespace Pedidos.Domain.Entities
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }

        public decimal TotalPrice
        {
            get { return UnitPrice * Quantity; }
        }

        protected OrderItem()
        {
        }

        public OrderItem(Product product, int quantity)
        {
            if (product == null)
                throw new ArgumentNullException("product");

            if (quantity <= 0)
                throw new ArgumentException("A quantidade deve ser maior que zero.", "quantity");

            ProductId = product.Id;
            ProductName = product.Name;
            UnitPrice = product.Price;
            Quantity = quantity;
        }
    }
}
