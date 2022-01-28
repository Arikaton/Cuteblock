namespace GameScripts.ConsumeSystem.Interfaces
{
    public interface IConsumable
    {
        bool CanConsume();
        void Consume();
        bool IsConsumed();
    }
}