using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;


namespace ComputerSeekho.API.Entities
{
    [Table("Recruiter_Master")]
    public class RecruiterMaster
    {
        [Key]
        [Column("recruiter_id")]
        public int RecruiterId { get; set; }

        [Required]
        [Column("recruiter_name")]
        [MaxLength(255)]
        public string? RecruiterName { get; set; }

        [Column("logo_url")]
        public string LogoUrl { get; set; }

        // Navigation Property
        // One Recruiter → Many Placements


        // ✅ New Field
        [Column("status")]
        public bool Status { get; set; } = true;

        [JsonIgnore]    
        public ICollection<PlacementMaster>? Placements { get; set; }
    }

}
