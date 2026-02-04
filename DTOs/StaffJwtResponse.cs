namespace ComputerSeekho.API.DTOs
{
    public class StaffJwtResponse
    {
        public string Token { get; set; }
        public int StaffId { get; set; }
        public string StaffUsername { get; set; }
        public string StaffEmail { get; set; }
        public string StaffName { get; set; }
        public string StaffRole { get; set; }

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
