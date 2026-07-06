import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';

/** Componente raiz: layout com header e o outlet das rotas. */
@Component({
  selector: 'app-root',
  imports: [RouterOutlet],
  templateUrl: './app.html'
})
export class App {
  protected readonly title = signal('Pedidos');
}
