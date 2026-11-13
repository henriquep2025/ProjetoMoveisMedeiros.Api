namespace ProjetoMoveisMedeiros.Api.Models
{
    public class Cliente
    {
        public int Id { get; set; }
        public string TipoPessoa { get; set; } // "Fisica" ou "Juridica"[cite: 1, 3, 5]
        public string Nome { get; set; } // Nome Completo do Titular ou Razão Social[cite: 1, 2, 3, 5]
        public string Documento { get; set; } // CPF ou CNPJ[cite: 1, 2, 3, 5]
        public DateTime? DataNascimento { get; set; } // Data de Nascimento ou Fundação[cite: 1, 3, 5]
        public string Email { get; set; } // E-mail Principal[cite: 1, 3, 5]
        public string Telefone { get; set; } // Telefone / WhatsApp Comercial[cite: 1, 3, 5]
        public string Cep { get; set; } // Exibido nos cards da listagem[cite: 3]
        public string Endereco { get; set; } // Endereço completo[cite: 1, 3, 5]
    }
}
