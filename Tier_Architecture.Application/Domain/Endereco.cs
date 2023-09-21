namespace Tier_Architecture.Application.Domain
{
    public class Endereco : Entity
    {
        public int Id { get; set; }
        public int EmpresaId { get; set; }
        public string Logradouro { get; set; }
        public string Numero { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string UF { get; set; }
        public bool Ativo { get; set; }
    }
}
