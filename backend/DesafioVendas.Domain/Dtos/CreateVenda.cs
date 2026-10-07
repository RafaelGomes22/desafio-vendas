namespace DesafioVendas.Domain.Dtos;

public class CreateVenda
{
    public string Produto { get; set; } = string.Empty;
    public int Quantidade { get; set; }
    public decimal PrecoUnitario { get; set; }
    public DateTime DataVenda { get; set; }
}
