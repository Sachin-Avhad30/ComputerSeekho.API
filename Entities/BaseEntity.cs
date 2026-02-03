using System.ComponentModel.DataAnnotations.Schema;

namespace ComputerSeekho.API.Entities
{
    public abstract class BaseEntity 
    {
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }
    }
}
