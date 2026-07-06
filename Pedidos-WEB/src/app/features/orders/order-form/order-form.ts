import { CurrencyPipe } from '@angular/common';
import { Component, OnInit, computed, inject, input, signal } from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';

import { OrderPayload } from '../../../core/models/order.model';
import { OrderService } from '../../../core/services/order.service';

// Estado do formulário: campos numéricos começam como null para os inputs
// virem vazios (em vez de mostrarem "0").
interface OrderItemFormModel {
  productId: number | null;
  productName: string;
  unitPrice: number | null;
  quantity: number;
}

interface OrderFormModel {
  clientName: string;
  clientEmail: string;
  isPaid: boolean;
  items: OrderItemFormModel[];
}

/**
 * Tela de criação/edição de pedido.
 *
 * Formulário template-driven: o estado fica no objeto `order`, ligado ao
 * template com [(ngModel)]; as validações vão como atributos no HTML.
 */
@Component({
  selector: 'app-order-form',
  imports: [FormsModule, RouterLink, CurrencyPipe],
  templateUrl: './order-form.html'
})
export class OrderForm implements OnInit {
  private readonly orderService = inject(OrderService);
  private readonly router = inject(Router);

  /** Parâmetro :id da rota (via withComponentInputBinding). Ausente = criação. */
  readonly id = input<string>();

  protected readonly saving = signal(false);
  protected readonly loading = signal(false);
  protected readonly error = signal<string | null>(null);
  protected readonly isEdit = computed(() => this.id() !== undefined);

  protected order: OrderFormModel = {
    clientName: '',
    clientEmail: '',
    isPaid: false,
    items: [this.newItem()]
  };

  /** Total em tempo real, recalculado a cada change detection. */
  protected get total(): number {
    return this.order.items.reduce(
      (sum, item) => sum + (Number(item.unitPrice) || 0) * (Number(item.quantity) || 0),
      0
    );
  }

  // Inputs de rota só ficam disponíveis a partir do ngOnInit (não no constructor).
  async ngOnInit(): Promise<void> {
    const id = this.id();
    if (id === undefined) {
      return;
    }

    this.loading.set(true);
    try {
      const order = await this.orderService.getById(Number(id));
      this.order = {
        clientName: order.clientName,
        clientEmail: order.clientEmail,
        isPaid: order.isPaid,
        items: order.items.map((item) => ({
          productId: item.productId,
          productName: item.productName,
          unitPrice: item.unitPrice,
          quantity: item.quantity
        }))
      };
    } catch {
      this.error.set('Pedido não encontrado.');
    } finally {
      this.loading.set(false);
    }
  }

  protected addItem(): void {
    this.order.items.push(this.newItem());
  }

  protected removeItem(index: number): void {
    if (this.order.items.length > 1) {
      this.order.items.splice(index, 1);
    }
  }

  protected async save(form: NgForm): Promise<void> {
    if (form.invalid) {
      // Marca tudo como "touched" para os erros aparecerem nos campos.
      form.form.markAllAsTouched();
      return;
    }

    this.saving.set(true);
    this.error.set(null);

    // Converte o estado do formulário no payload da API (garante números).
    const payload: OrderPayload = {
      clientName: this.order.clientName,
      clientEmail: this.order.clientEmail,
      isPaid: this.order.isPaid,
      items: this.order.items.map((item) => ({
        productId: Number(item.productId),
        productName: item.productName,
        unitPrice: Number(item.unitPrice),
        quantity: Number(item.quantity)
      }))
    };

    try {
      const id = this.id();
      if (id !== undefined) {
        await this.orderService.update(Number(id), payload);
      } else {
        await this.orderService.create(payload);
      }
      this.router.navigate(['/pedidos']);
    } catch (err) {
      const detail = (err as { error?: { detail?: string } })?.error?.detail;
      this.error.set(detail ?? 'Não foi possível salvar o pedido.');
      this.saving.set(false);
    }
  }

  private newItem(): OrderItemFormModel {
    return { productId: null, productName: '', unitPrice: null, quantity: 1 };
  }
}
