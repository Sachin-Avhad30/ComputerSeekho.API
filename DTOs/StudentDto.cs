using ComputerSeekho.API.Enum;

namespace ComputerSeekho.API.DTOs
{
    public class StudentDto
    {
        public int? BatchId { get; set; }
        public int? CourseId { get; set; }

        public string StudentName { get; set; }
        public long StudentMobile { get; set; }

        public string StudentGender { get; set; }
        public DateTime? StudentDob { get; set; }
        public string StudentAddress { get; set; }
        public string StudentQualification { get; set; }

        public string StudentUsername { get; set; }
        public string StudentPassword { get; set; }

        //public string PhotoUrl { get; set; }

        public RegistrationStatus RegistrationStatus { get; set; }
            = RegistrationStatus.PaymentPending;

        public IFormFile? Photo { get; set; }

    }
}
