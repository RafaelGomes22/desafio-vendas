import { Component, ViewChild } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { Venda, VendaAgregada } from '../../core/models/venda.model';
import { CsvService } from '../../core/services/csv.service';
import { VendasService } from '../../core/services/vendas.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html'
})
export class DashboardComponent {
  vendas: Venda[] = [];
  agregadas: VendaAgregada[] = [];
  filtradas: VendaAgregada[] = [];
  filtroProduto = '';
  produtoSelecionado: VendaAgregada | null = null;

  @ViewChild('detalhesModal') detalhesModal?: ModalDirective;

  chartData = { labels: [] as string[], datasets: [{ label: 'Quantidade', data: [] as number[] }] };
  chartOptions = { responsive: true, maintainAspectRatio: false };

  constructor(private csv: CsvService, private api: VendasService) {}

  carregar(vendas: Venda[]): void {
    this.vendas = vendas;
    this.agregadas = this.csv.agrupar(vendas);
    this.aplicarFiltro();
    this.chartData = {
      labels: this.filtradas.map(x => x.produto),
      datasets: [{ label: 'Quantidade', data: this.filtradas.map(x => x.quantidade) }]
    };

    vendas.forEach(v => this.api.criar(v).subscribe({
      error: err => console.warn('API não disponível ou venda já existente.', err)
    }));
  }

  aplicarFiltro(): void {
    const termo = this.filtroProduto.trim().toLowerCase();
    this.filtradas = termo
      ? this.agregadas.filter(x => x.produto.toLowerCase().includes(termo))
      : [...this.agregadas];
  }

  selecionar(item: VendaAgregada): void {
    this.produtoSelecionado = item;
    this.detalhesModal?.show();
  }

  exportar(): void {
    const linhas = [
      'produto,quantidade,valor_total',
      ...this.filtradas.map(x => `${x.produto},${x.quantidade},${x.valorTotal.toFixed(2)}`)
    ];
    const blob = new Blob([linhas.join('\n')], { type: 'text/csv;charset=utf-8' });
    const url = URL.createObjectURL(blob);
    const a = document.createElement('a');
    a.href = url;
    a.download = 'vendas-agregadas.csv';
    a.click();
    URL.revokeObjectURL(url);
  }
}
