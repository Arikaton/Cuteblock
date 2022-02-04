namespace GameScripts.Producible
{
    public interface IProducible
    {
        void Produce();
        bool IsProduced { get; }
        string ResourceId { get; }
        int Amount { get; }
    }
}