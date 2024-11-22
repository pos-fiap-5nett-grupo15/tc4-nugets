namespace TechChallenge3.Infrastructure.UnitOfWork
{
    public interface ITransaction : IDisposable
    {
        void Commit();
        void Rollback();
    }
}
