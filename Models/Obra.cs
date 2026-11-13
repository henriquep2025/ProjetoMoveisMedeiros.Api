namespace ProjetoMoveisMedeiros.Api.Models
{
    public class Obra
    {
        public int Id { get; set; }
        public string NomeProjeto { get; set; } 
        public string Status { get; set; } = "EM_ANDAMENTO"; 
        public DateTime? DataPrevistaInicio { get; set; }
        public DateTime? DataEstimadaEntrega { get; set; }
        public DateTime? DataEntregaReal { get; set; } 
        public string AmbientesContratados { get; set; } 
        public string MateriaisInsumos { get; set; } 
        public decimal Preco { get; set; } 
        public string? Lote { get; set; } 
        public string? AmbienteMovel { get; set; } 
        public string? TipoMovel { get; set; } 
        public string? DescricaoMovel { get; set; } 
        public string? CorAcabamento { get; set; } 
        public string? Dimensoes { get; set; } 
        public string? ObservacoesProjeto { get; set; }
        public string? ChecklistJson { get; set; }
        public DateTime? DataValidacaoChecklist { get; set; }
        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; }
    }
}