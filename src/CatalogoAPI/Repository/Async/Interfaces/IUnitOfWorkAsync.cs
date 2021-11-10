using System.Threading.Tasks;

namespace CatalogoAPI.Repository.Async.Interfaces
{
    public interface IUnitOfWorkAsync
    {
        IProdutoRepositoryAsync ProdutoRepositoryAsync { get; }
        Task CommitAsync();
        Task DisposeAsync();

    }
}
