namespace ComputerSeekho.API.DTOs
{
    public class StaffInfoResponse
    {
        public int StaffId { get; set; }
        public string StaffName { get; set; }
        public string PhotoUrl { get; set; }
        public string StaffMobile { get; set; }
        public string StaffEmail { get; set; }
        public string StaffUsername { get; set; }
        public string StaffRole { get; set; }
        public bool IsActive { get; set; }
        public string StaffBio { get; set; }
        public string StaffDesignation { get; set; }
    }
}
