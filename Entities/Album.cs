using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComputerSeekho.API.Entities
{
    [Table("album_master")]

    public class Album
    {
        [Key]
        [Column("album_id")]
        public int AlbumId { get; set; }

        [Required]
        [MaxLength(150)]
        [Column("album_name")]
        public string? AlbumName { get; set; }

        [MaxLength(500)]
        [Column("album_description")]
        public string? AlbumDescription { get; set; }

        [Column("start_date")]
        public DateTime? StartDate { get; set; }

        [Column("end_date")]
        public DateTime? EndDate { get; set; }

        [Column("album_is_active")]
        public bool AlbumIsActive { get; set; } = true;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }

        // Navigation Property
        public ICollection<Image> Images { get; set; }
    }
}
