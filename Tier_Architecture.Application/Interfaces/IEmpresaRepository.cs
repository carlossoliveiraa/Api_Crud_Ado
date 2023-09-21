using System.Linq.Expressions;
using Tier_Architecture.Application.Domain;

namespace Tier_Architecture.Application.Interfaces
{
    public interface IEmpresaRepository
    {
        Task Adicionar(Empresa empresa);
        Task<Empresa> ObterPorId(Int32 id);
        Task<IEnumerable<Empresa>> ObterTodos();
        Task Atualizar(Empresa empresa);
        Task Remover(Int32 id);          
    }
}
