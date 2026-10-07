import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Venda } from '../models/venda.model';

@Injectable({ providedIn: 'root' })
export class VendasService {
  private readonly apiUrl = 'https://localhost:7043/api/vendas';

  constructor(private http: HttpClient) {}

  listar(quantidadeMin?: number, quantidadeMax?: number, dataInicial?: string, dataFinal?: string): Observable<Venda[]> {
    let params = new HttpParams();

    if (quantidadeMin != null) params = params.set('quantidadeMin', quantidadeMin);
    if (quantidadeMax != null) params = params.set('quantidadeMax', quantidadeMax);
    if (dataInicial) params = params.set('dataInicial', dataInicial);
    if (dataFinal) params = params.set('dataFinal', dataFinal);

    return this.http.get<Venda[]>(this.apiUrl, { params });
  }

  criar(venda: Venda): Observable<Venda> {
    return this.http.post<Venda>(this.apiUrl, venda);
  }
}
