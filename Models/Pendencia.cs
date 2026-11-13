namespace ProjetoMoveisMedeiros.Api.Models
{
    public class Pendencia
    {
        public int Id { get; set; }
        public string PontoIdentificado { get; set; } // Ex: "Cozinha Gourmet • Torre de Embutir Nicho Forno"
        public string DescricaoOcorrencia { get; set; } // Descrição Detalhada da Ocorrência
        public string AcaoCorretiva { get; set; } // Ação Corretiva Imediata Recomendada[cite: 11]
        public string FotosUrls { get; set; } // Caminhos das 2 fotos obrigatórias (separados por ";" ou JSON)[cite: 11]
        public string Status { get; set; } = "ABERTA"; // "ABERTA", "EM_ANALISE", "RESOLVIDA"
        public string GrauSeveridade { get; set; } = "LEVE"; // "LEVE", "MÉDIA", "CRÍTICA"

        public int ObraId { get; set; }
        public Obra Obra { get; set; }


    }
}
