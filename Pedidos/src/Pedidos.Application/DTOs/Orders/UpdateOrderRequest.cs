using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Pedidos.Application.DTOs.Orders
{
    // Os atributos JsonPropertyName mantêm o contrato JSON em português exigido pelo desafio.
    public class UpdateOrderRequest
    {
        public UpdateOrderRequest()
        {
            Items = new List<OrderItemRequest>();
        }

        [JsonPropertyName("nomeCliente")]
        public string ClientName { get; set; }

        [JsonPropertyName("emailCliente")]
        public string ClientEmail { get; set; }

        [JsonPropertyName("pago")]
        public bool IsPaid { get; set; }

        [JsonPropertyName("itensPedido")]
        public List<OrderItemRequest> Items { get; set; }
    }
}
