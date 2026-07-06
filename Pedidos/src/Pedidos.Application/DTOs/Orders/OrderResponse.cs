using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Pedidos.Application.DTOs.Orders
{
    // Os atributos JsonPropertyName mantêm o contrato JSON em português exigido pelo desafio.
    public class OrderResponse
    {
        public OrderResponse()
        {
            Items = new List<OrderItemResponse>();
        }

        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("nomeCliente")]
        public string ClientName { get; set; }

        [JsonPropertyName("emailCliente")]
        public string ClientEmail { get; set; }

        [JsonPropertyName("pago")]
        public bool IsPaid { get; set; }

        [JsonPropertyName("valorTotal")]
        public decimal TotalPrice { get; set; }

        [JsonPropertyName("itensPedido")]
        public List<OrderItemResponse> Items { get; set; }
    }
}
