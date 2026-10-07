import { Component, EventEmitter, Output } from '@angular/core';
import { CsvService } from '../../core/services/csv.service';
import { Venda } from '../../core/models/venda.model';

@Component({
  selector: 'app-upload',
  templateUrl: './upload.component.html'
})
export class UploadComponent {
  @Output() vendasImportadas = new EventEmitter<Venda[]>();
  erro = '';
  sucesso = '';

  constructor(private csvService: CsvService) {}

  selecionarArquivo(event: Event): void {
    this.erro = '';
    this.sucesso = '';

    const input = event.target as HTMLInputElement;
    const file = input.files?.[0];
    if (!file) return;

    if (!file.name.toLowerCase().endsWith('.csv')) {
      this.erro = 'Selecione um arquivo CSV.';
      return;
    }

    const reader = new FileReader();
    reader.onload = () => {
      try {
        const vendas = this.csvService.parse(String(reader.result || ''));
        localStorage.setItem('ultimoCsvVendas', JSON.stringify(vendas));
        this.vendasImportadas.emit(vendas);
        this.sucesso = `${vendas.length} venda(s) importada(s) com sucesso.`;
      } catch (e) {
        this.erro = e instanceof Error ? e.message : 'Erro ao processar CSV.';
      }
    };
    reader.onerror = () => this.erro = 'Não foi possível ler o arquivo.';
    reader.readAsText(file, 'UTF-8');
  }
}
