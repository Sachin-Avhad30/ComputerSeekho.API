using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ComputerSeekho.API.Entities
{
    [Table("Placements_Master")]
    public class Placement
    {
        [Key]
        [Column("placement_id")]
        public int PlacementId { get; set; }

        [Column("student_id")]
        public int StudentId { get; set; }

        [Column("batch_id")]
        public int BatchId { get; set; }

        [Column("recruiter_id")]
        public int RecruiterId { get; set; }

        [ForeignKey(nameof(StudentId))]
        public Student? Student { get; set; }

        [ForeignKey(nameof(BatchId))]
        public Batch? Batch { get; set; }

        [ForeignKey(nameof(RecruiterId))]
        public Recruiter? Recruiter { get; set; }
    
}
}
