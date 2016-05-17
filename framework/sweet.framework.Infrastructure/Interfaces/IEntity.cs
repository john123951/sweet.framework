namespace sweet.framework.Infrastructure.Interfaces
{
    public interface IEntity<TKey>
        where TKey : struct
    {
        TKey Id { get; set; }
    }
}