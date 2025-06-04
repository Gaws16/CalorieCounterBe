namespace CalorieCounterBe.Core.Contracts
{
    public interface IGeminiService
    {
        Task<string> SendMessageAsync(string message);
    }
}
