import { CurrencyPipe } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { RouterLink } from '@angular/router';

import { Order } from '../../../core/models/order.model';
import { OrderService } from '../../../core/services/order.service';

/** Tela de listagem de pedidos. */
@Component({
  selector: 'app-order-list',
  imports: [CurrencyPipe, RouterLink],
  templateUrl: './order-list.html'
})
export class OrderList {
  private readonly orderService = inject(OrderService);

  protected readonly orders = signal<Order[]>([]);
  protected readonly loading = signal(true);
  protected readonly error = signal<string | null>(null);
  protected readonly expandedId = signal<number | null>(null);

  constructor() {
    void this.load();
  }

  protected async load(): Promise<void> {
    this.loading.set(true);
    this.error.set(null);
    try {
      this.orders.set(await this.orderService.getAll());
    } catch {
      this.error.set('Não foi possível carregar os pedidos. Verifique se a API está em execução.');
    } finally {
      this.loading.set(false);
    }
  }

  /** Expande/recolhe os itens de um pedido na tabela. */
  protected toggleItems(id: number): void {
    this.expandedId.set(this.expandedId() === id ? null : id);
  }

  protected async remove(order: Order): Promise<void> {
    if (!confirm(`Excluir o pedido #${order.id} de ${order.clientName}?`)) {
      return;
    }

    try {
      await this.orderService.delete(order.id);
      this.orders.update((orders) => orders.filter((o) => o.id !== order.id));
    } catch {
      this.error.set('Não foi possível excluir o pedido.');
    }
  }
}
