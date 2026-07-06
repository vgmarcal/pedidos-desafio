using System;
using System.Collections.Generic;
using System.Linq;

namespace Pedidos.Domain.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public string ClientName { get; set; }
        public string ClientEmail { get; set; }
        public DateTime OrderDate { get; set; }
        public bool IsPaid { get; set; }
        public List<OrderItem> OrderItems { get; set; }

        public decimal TotalPrice
        {
            get { return OrderItems.Sum(item => item.TotalPrice); }
        }

        protected Order()
        {
            OrderItems = new List<OrderItem>();
        }

        public Order(string clientName, string clientEmail, bool isPaid)
        {
            ValidateClient(clientName, clientEmail);

            ClientName = clientName.Trim();
            ClientEmail = clientEmail.Trim();
            IsPaid = isPaid;
            OrderDate = DateTime.UtcNow;
            OrderItems = new List<OrderItem>();
        }

        public void UpdateClient(string clientName, string clientEmail)
        {
            ValidateClient(clientName, clientEmail);

            ClientName = clientName.Trim();
            ClientEmail = clientEmail.Trim();
        }

        public void SetPaid(bool isPaid)
        {
            IsPaid = isPaid;
        }

        public void AddOrderItem(Product product, int quantity)
        {
            if (product == null)
                throw new ArgumentNullException("product");

            OrderItems.Add(new OrderItem(product, quantity));
        }

        public void ClearOrderItems()
        {
            OrderItems.Clear();
        }

        private static void ValidateClient(string clientName, string clientEmail)
        {
            if (string.IsNullOrWhiteSpace(clientName))
                throw new ArgumentException("O nome do cliente é obrigatório", "clientName");

            if (string.IsNullOrWhiteSpace(clientEmail))
                throw new ArgumentException("O e-mail do cliente é obrigatório", "clientEmail");

            if (!clientEmail.Contains("@"))
                throw new ArgumentException("O e-mail do cliente é inválido", "clientEmail");
        }
    }
}
