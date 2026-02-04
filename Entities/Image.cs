using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComputerSeekho.API.Entities
{
    [Table("image_master")]
    public class Image
    {
        [Key]
        [Column("image_id")]
        public int ImageId { get; set; }

        [Required]
        [MaxLength(255)]
        [Column("image_path")]
        public string ImagePath { get; set; }

        [ForeignKey("AlbumMaster")]
        [Column("album_id")]
        public int AlbumId { get; set; }
        public Album Album { get; set; }

        [Column("is_album_cover")]
        public bool IsAlbumCover { get; set; }

        [Column("image_is_active")]
        public bool ImageIsActive { get; set; } = true;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        // Navigation Property
    }
}
