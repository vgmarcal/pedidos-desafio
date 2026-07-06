import { registerLocaleData } from '@angular/common';
import { provideHttpClient } from '@angular/common/http';
import localePt from '@angular/common/locales/pt';
import { ApplicationConfig, LOCALE_ID, provideBrowserGlobalErrorListeners } from '@angular/core';
import { provideRouter, withComponentInputBinding } from '@angular/router';

import { routes } from './app.routes';

registerLocaleData(localePt);

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    // withComponentInputBinding: parâmetros de rota viram inputs do componente.
    provideRouter(routes, withComponentInputBinding()),
    provideHttpClient(),
    // Locale pt-BR para o pipe currency formatar em R$.
    { provide: LOCALE_ID, useValue: 'pt-BR' }
  ]
};
