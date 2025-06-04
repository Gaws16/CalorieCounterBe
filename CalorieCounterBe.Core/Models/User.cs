using Supabase.Postgrest.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalorieCounterBe.Core.Models
{
    [Table("users")]
    public class User : BaseModel
    {
        [Column("id")]
        public string Id { get; set; } = string.Empty;
        [Column("email")]
        public string Email { get; set; } = string.Empty;
        [Column("full_name")]
        public string FullName { get; set; } = string.Empty;
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }
        // Additional properties can be added as needed
    }
}
