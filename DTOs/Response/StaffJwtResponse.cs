namespace ComputerSeekho.API.DTOs.Response
{
    public class StaffJwtResponse
    {
        public string Token { get; set; } = string.Empty;
        public string Type { get; set; } = "Bearer";
        public int StaffId { get; set; }
        public string StaffUsername { get; set; } = string.Empty;
        public string StaffEmail { get; set; } = string.Empty;
        public string StaffName { get; set; } = string.Empty;
        public string StaffRole { get; set; } = string.Empty;

        public StaffJwtResponse() { }

        public StaffJwtResponse(string token, int staffId, string staffUsername,
                                string staffEmail, string staffName, string staffRole)
        {
            Token = token;
            StaffId = staffId;
            StaffUsername = staffUsername;
            StaffEmail = staffEmail;
            StaffName = staffName;
            StaffRole = staffRole;
        }
    }
}