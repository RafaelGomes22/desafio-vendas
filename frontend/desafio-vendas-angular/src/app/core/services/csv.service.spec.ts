import { CsvService } from './csv.service';

describe('CsvService', () => {
  let service: CsvService;

  beforeEach(() => service = new CsvService());

  it('deve fazer parse do CSV', () => {
    const vendas = service.parse(
      'id_venda,produto,quantidade,preco_unitario,data_venda\n' +
      '1,Camiseta,3,49.90,06/09/2026'
    );

    expect(vendas.length).toBe(1);
    expect(vendas[0].produto).toBe('Camiseta');
    expect(vendas[0].quantidade).toBe(3);
  });

  it('deve agrupar produtos', () => {
    const vendas = service.parse(
      'id_venda,produto,quantidade,preco_unitario,data_venda\n' +
      '1,Camiseta,3,49.90,06/09/2026\n' +
      '3,Camiseta,1,49.90,06/09/2026'
    );

    const result = service.agrupar(vendas);
    expect(result[0].quantidade).toBe(4);
    expect(result[0].valorTotal).toBeCloseTo(199.60, 2);
  });
});
