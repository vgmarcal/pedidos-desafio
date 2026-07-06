// DTOs: formato exato do JSON trafegado com a API (campos em pt-BR, como os
// DTOs do backend). Só o OrderService conhece estes tipos; o resto da
// aplicação usa os modelos em inglês de order.model.ts.

import { Order, OrderItem, OrderPayload } from './order.model';

export interface OrderItemDto {
  id: number;
  idProduto: number;
  nomeProduto: string;
  valorUnitario: number;
  quantidade: number;
}

export interface OrderDto {
  id: number;
  nomeCliente: string;
  emailCliente: string;
  pago: boolean;
  valorTotal: number;
  itensPedido: OrderItemDto[];
}

export interface OrderItemPayloadDto {
  idProduto: number;
  nomeProduto: string;
  valorUnitario: number;
  quantidade: number;
}

export interface OrderPayloadDto {
  nomeCliente: string;
  emailCliente: string;
  pago: boolean;
  itensPedido: OrderItemPayloadDto[];
}

export function toOrder(dto: OrderDto): Order {
  return {
    id: dto.id,
    clientName: dto.nomeCliente,
    clientEmail: dto.emailCliente,
    isPaid: dto.pago,
    totalPrice: dto.valorTotal,
    items: dto.itensPedido.map(toOrderItem)
  };
}

function toOrderItem(dto: OrderItemDto): OrderItem {
  return {
    id: dto.id,
    productId: dto.idProduto,
    productName: dto.nomeProduto,
    unitPrice: dto.valorUnitario,
    quantity: dto.quantidade
  };
}

export function toOrderPayloadDto(payload: OrderPayload): OrderPayloadDto {
  return {
    nomeCliente: payload.clientName,
    emailCliente: payload.clientEmail,
    pago: payload.isPaid,
    itensPedido: payload.items.map((item) => ({
      idProduto: item.productId,
      nomeProduto: item.productName,
      valorUnitario: item.unitPrice,
      quantidade: item.quantity
    }))
  };
}
