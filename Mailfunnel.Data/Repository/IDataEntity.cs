namespace Mailfunnel.Data.Repository
{
    public interface IDataEntity<TKey>
    {
        TKey Id { get; set; }
    }
}