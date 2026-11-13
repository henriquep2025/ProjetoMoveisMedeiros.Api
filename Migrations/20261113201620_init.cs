using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjetoMoveisMedeiros.Api.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clientes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TipoPessoa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Documento = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataNascimento = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Telefone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cep = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Endereco = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clientes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Obras",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomeProjeto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataPrevistaInicio = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DataEstimadaEntrega = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DataEntregaReal = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AmbientesContratados = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MateriaisInsumos = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Preco = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Lote = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AmbienteMovel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TipoMovel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescricaoMovel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CorAcabamento = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Dimensoes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ObservacoesProjeto = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChecklistJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataValidacaoChecklist = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ClienteId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Obras", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Obras_Clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Clientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pendencias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PontoIdentificado = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DescricaoOcorrencia = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AcaoCorretiva = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FotosUrls = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GrauSeveridade = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ObraId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pendencias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pendencias_Obras_ObraId",
                        column: x => x.ObraId,
                        principalTable: "Obras",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Obras_ClienteId",
                table: "Obras",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Pendencias_ObraId",
                table: "Pendencias",
                column: "ObraId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Pendencias");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Obras");

            migrationBuilder.DropTable(
                name: "Clientes");
        }
    }
}
