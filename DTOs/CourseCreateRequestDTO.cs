using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerSeekho.Application.DTOs
{
    public class CourseCreateRequestDTO
    {
        [Required]
        public string CourseName { get; set; }
        public string? CourseDescription { get; set; }
        public int? CourseDuration { get; set; }
        public decimal? CourseFees { get; set; }
        public DateTime? CourseFeesFrom { get; set; }
        public DateTime? CourseFeesTo { get; set; }
        public string? CourseSyllabus { get; set; }
        public string? AgeGrpType { get; set; }
        public int? VideoId { get; set; }
        public IFormFile CoverPhoto { get; set; }
    }
}
