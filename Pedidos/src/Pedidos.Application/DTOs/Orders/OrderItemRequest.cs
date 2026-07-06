using System.Text.Json.Serialization;

namespace Pedidos.Application.DTOs.Orders
{
    public class OrderItemRequest
    {
        [JsonPropertyName("idProduto")]
        public int ProductId { get; set; }

        [JsonPropertyName("nomeProduto")]
        public string ProductName { get; set; }

        [JsonPropertyName("valorUnitario")]
        public decimal UnitPrice { get; set; }

        [JsonPropertyName("quantidade")]
        public int Quantity { get; set; }
    }
}
