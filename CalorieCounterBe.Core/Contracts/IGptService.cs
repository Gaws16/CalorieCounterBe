namespace CalorieCounterBe.Core.Contracts
{
    public interface IGptService
    {
        /// <summary>
        /// Sends a message to the GPT service and returns the response.
        /// </summary>
        /// <param name="message">The message to send to the GPT service.</param>
        /// <returns>The response from the GPT service.</returns>
        Task<string> SendMessageAsync(string message);
        /// <summary>
        /// Gets the status of the GPT service.
        /// </summary>
        /// <returns>A string indicating the status of the GPT service.</returns>
        Task<string> GetStatusAsync();
    }
}
