using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace CalorieCounterBe.Core.Models
{
    [Table("Test")]
    public class Test : BaseModel
    {
        [PrimaryKey("id")]
        public int Id { get; set; }
        [Column("ime")]
        public string Ime { get; set; } = string.Empty;

    }
}
