namespace CatalogoAPI.Repository.Interfaces
{
    public interface IUnitOfWork
    {
        ICategoriaRepository CategoriaRepository { get; }
        void Commit();
        void Dispose();

    }
}
