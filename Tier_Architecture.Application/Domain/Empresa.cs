using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tier_Architecture.Application.Domain
{
    public class Empresa : Entity
    {
        public string Nome { get; set; }
        public string Cnpj { get; set; }
        public string InscricaoEstadual { get; set; }
        public DateTime DataAbertura { get; set; }
        public string Site { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public bool Ativo { get; set; }

        public List<Endereco> Enderecos { get; set; } 
    }
}
