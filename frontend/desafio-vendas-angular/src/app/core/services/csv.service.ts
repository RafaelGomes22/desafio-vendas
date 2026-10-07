import { Injectable } from '@angular/core';
import { Venda } from '../models/venda.model';

@Injectable({ providedIn: 'root' })
export class CsvService {
  private readonly headers = ['id_venda', 'produto', 'quantidade', 'preco_unitario', 'data_venda'];

  parse(csv: string): Venda[] {
    const lines = csv.split(/\r?\n/).map(x => x.trim()).filter(Boolean);
    if (lines.length < 2) throw new Error('CSV vazio ou sem dados.');

    const header = lines[0].split(',').map(x => x.trim().toLowerCase());
    if (header.join('|') !== this.headers.join('|')) {
      throw new Error('Cabeçalho inválido. Esperado: id_venda,produto,quantidade,preco_unitario,data_venda');
    }

    return lines.slice(1).map((line, index) => {
      const parts = line.split(',');
      if (parts.length !== 5) throw new Error(`Linha ${index + 2}: quantidade de colunas inválida.`);

      const quantidade = Number(parts[2]);
      const precoUnitario = Number(parts[3].replace(',', '.'));
      const dataVenda = this.parseDate(parts[4]);

      if (!Number.isInteger(Number(parts[0])) || !Number.isInteger(quantidade) || Number.isNaN(precoUnitario)) {
        throw new Error(`Linha ${index + 2}: valor numérico inválido.`);
      }

      return {
        idVenda: Number(parts[0]),
        produto: parts[1].trim(),
        quantidade,
        precoUnitario,
        dataVenda
      };
    });
  }

  private parseDate(value: string): string {
    const [day, month, year] = value.trim().split('/');
    if (!day || !month || !year) throw new Error(`Data inválida: ${value}`);
    return `${year}-${month.padStart(2, '0')}-${day.padStart(2, '0')}`;
  }

  agrupar(vendas: Venda[]): { produto: string; quantidade: number; valorTotal: number }[] {
    const map = new Map<string, { produto: string; quantidade: number; valorTotal: number }>();

    vendas.forEach(v => {
      const atual = map.get(v.produto) || { produto: v.produto, quantidade: 0, valorTotal: 0 };
      atual.quantidade += v.quantidade;
      atual.valorTotal += v.quantidade * v.precoUnitario;
      map.set(v.produto, atual);
    });

    return Array.from(map.values());
  }
}
