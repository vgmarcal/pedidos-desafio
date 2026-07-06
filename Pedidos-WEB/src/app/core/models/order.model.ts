// Modelos usados pela aplicação (inglês, seguindo as entities do backend).
// O formato pt-BR do JSON da API fica isolado nos DTOs (order.dto.ts).

/** Item de um pedido (inclui o id gerado pelo backend). */
export interface OrderItem {
  id: number;
  productId: number;
  productName: string;
  unitPrice: number;
  quantity: number;
}

/** Pedido como a aplicação usa (totalPrice calculado no backend). */
export interface Order {
  id: number;
  clientName: string;
  clientEmail: string;
  isPaid: boolean;
  totalPrice: number;
  items: OrderItem[];
}

/** Item enviado na criação/atualização (sem id — quem gera é o backend). */
export interface OrderItemPayload {
  productId: number;
  productName: string;
  unitPrice: number;
  quantity: number;
}

/** Dados enviados na criação/atualização de pedido. */
export interface OrderPayload {
  clientName: string;
  clientEmail: string;
  isPaid: boolean;
  items: OrderItemPayload[];
}
