using CalorieCounterBe.Core.Models;

namespace CalorieCounterBe.Core.Services
{
    public class SupabaseService
    {
        private readonly Supabase.Client supabaseClient;

        public SupabaseService(Supabase.Client supabaseClient)
        {
            this.supabaseClient = supabaseClient ?? throw new ArgumentNullException(nameof(supabaseClient), "Supabase client cannot be null.");
        }

        public async Task<List<Test>> GetUsers()
        {
            var response = await supabaseClient
                 .From<Test>()
                 .Get();
            

            return response.Models;
        }



    }
}
