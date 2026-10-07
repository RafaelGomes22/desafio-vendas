export interface Venda {
  idVenda: number;
  produto: string;
  quantidade: number;
  precoUnitario: number;
  dataVenda: string | Date;
}

export interface VendaAgregada {
  produto: string;
  quantidade: number;
  valorTotal: number;
}
