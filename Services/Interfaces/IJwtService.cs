using ComputerSeekho.API.Entities;

namespace ComputerSeekho.Application.Services.Interfaces
{
    public interface IJwtService
    {
        string GenerateJwtToken(Staff staff);
        string GenerateTokenFromUsername(string username);
        string? GetUsernameFromToken(string token);
        bool ValidateJwtToken(string token);
    }
}