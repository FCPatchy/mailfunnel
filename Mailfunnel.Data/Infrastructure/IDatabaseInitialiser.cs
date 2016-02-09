namespace Mailfunnel.Data.Infrastructure
{
    public interface IDatabaseInitialiser
    {
        void InitialiseDatabase();
        void SeedDatabase();
    }
}