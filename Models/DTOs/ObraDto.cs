using System.ComponentModel.DataAnnotations;

namespace ProjetoMoveisMedeiros.Api.Models.DTOs
{
    public class ObraDto
    {
        public int Id { get; set; }
        public string? NomeProjeto { get; set; } 
        public string? Status { get; set; } 
        public DateTime? DataPrevistaInicio { get; set; }
        public DateTime? DataEstimadaEntrega { get; set; }
        public DateTime? DataEntregaReal { get; set; }
        public string? AmbientesContratados { get; set; } 
        public string? MateriaisInsumos { get; set; } 
        public decimal Preco { get; set; }
        public string?  Lote { get; set; }
        public string? AmbienteMovel { get; set; }
        public string? TipoMovel { get; set; }
        public string? DescricaoMovel { get; set; }
        public string? CorAcabamento { get; set; }
        public string? Dimensoes { get; set; }
        public string? ObservacoesProjeto { get; set; }
        public string? ChecklistJson { get; set; }
        public DateTime? DataValidacaoChecklist { get; set; }
        public int ClienteId { get; set; }
    }

    public class CriarObraDto
    {
        [Required(ErrorMessage = "O nome do projeto é obrigatório.")]
        public string NomeProjeto { get; set; } = string.Empty;

        public string Status { get; set; } = "EM_ANDAMENTO";
        public DateTime? DataPrevistaInicio { get; set; }
        public DateTime? DataEstimadaEntrega { get; set; }

        [Required(ErrorMessage = "Os ambientes contratados são obrigatórios.")]
        public string AmbientesContratados { get; set; } = string.Empty;

        public string MateriaisInsumos { get; set; } = string.Empty;

        [Range(0.01, double.MaxValue, ErrorMessage = "O preço deve ser maior que zero.")]
        public decimal Preco { get; set; }

        public string? Lote { get; set; }
        public string? AmbienteMovel { get; set; }
        public string? TipoMovel { get; set; }
        public string? DescricaoMovel { get; set; }
        public string? CorAcabamento { get; set; }
        public string? Dimensoes { get; set; }
        public string? ObservacoesProjeto { get; set; }

        [Required(ErrorMessage = "O ID do cliente é obrigatório.")]
        public int ClienteId { get; set; }
    }
}    