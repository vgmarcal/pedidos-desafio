import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { firstValueFrom } from 'rxjs';

import { OrderDto, toOrder, toOrderPayloadDto } from '../models/order.dto';
import { Order, OrderPayload } from '../models/order.model';

/**
 * Serviço de acesso à API de pedidos.
 *
 * Converte os DTOs pt-BR da API nos modelos em inglês da aplicação e
 * transforma os Observables do HttpClient em Promises (`firstValueFrom`)
 * para permitir `async/await` nos componentes.
 */
@Injectable({ providedIn: 'root' })
export class OrderService {
  private readonly http = inject(HttpClient);

  // Em dev o proxy (proxy.conf.json) redireciona /api/* para a API .NET em
  // http://localhost:8080 (docker compose); no Docker, o nginx faz o mesmo papel.
  private readonly baseUrl = '/api/pedidos';

  async getAll(): Promise<Order[]> {
    const dtos = await firstValueFrom(this.http.get<OrderDto[]>(this.baseUrl));
    return dtos.map(toOrder);
  }

  async getById(id: number): Promise<Order> {
    const dto = await firstValueFrom(this.http.get<OrderDto>(`${this.baseUrl}/${id}`));
    return toOrder(dto);
  }

  async create(payload: OrderPayload): Promise<Order> {
    const dto = await firstValueFrom(this.http.post<OrderDto>(this.baseUrl, toOrderPayloadDto(payload)));
    return toOrder(dto);
  }

  async update(id: number, payload: OrderPayload): Promise<Order> {
    const dto = await firstValueFrom(this.http.put<OrderDto>(`${this.baseUrl}/${id}`, toOrderPayloadDto(payload)));
    return toOrder(dto);
  }

  delete(id: number): Promise<void> {
    return firstValueFrom(this.http.delete<void>(`${this.baseUrl}/${id}`));
  }
}
