import { Routes } from '@angular/router';

import { OrderForm } from './features/orders/order-form/order-form';
import { OrderList } from './features/orders/order-list/order-list';

// O parâmetro :id chega ao componente como input, graças ao
// withComponentInputBinding em app.config.ts.
export const routes: Routes = [
  { path: '', redirectTo: 'pedidos', pathMatch: 'full' },
  { path: 'pedidos', component: OrderList },
  { path: 'pedidos/novo', component: OrderForm },
  { path: 'pedidos/:id/editar', component: OrderForm },
  { path: '**', redirectTo: 'pedidos' }
];
