using ComputerSeekho.API.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ComputerSeekho.API.Security.JWT
{
    public interface IJwtUtils
    {
        string GenerateJwtToken(Staff staff);
        int? ValidateJwtToken(string token);
    }

    public class JwtUtils : IJwtUtils
    {
        private readonly string _secret;
        private readonly int _expirationInHours;

        public JwtUtils(IConfiguration configuration)
        {
            _secret = configuration["JwtSettings:Secret"];
            _expirationInHours = int.Parse(configuration["JwtSettings:ExpirationInHours"] ?? "24");
        }

        public string GenerateJwtToken(Staff staff)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secret);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, staff.StaffId.ToString()),
                new Claim(ClaimTypes.Name, staff.StaffUsername),
                new Claim(ClaimTypes.Email, staff.StaffEmail),
                new Claim("staffName", staff.StaffName),
                new Claim(ClaimTypes.Role, staff.StaffRole),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(_expirationInHours),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                ),
                Issuer = "ComputerSeekhoApi",
                Audience = "ComputerSeekhoClient"
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public int? ValidateJwtToken(string token)
        {
            if (string.IsNullOrEmpty(token))
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secret);

            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = "ComputerSeekhoApi",
                    ValidateAudience = true,
                    ValidAudience = "ComputerSeekhoClient",
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var staffId = int.Parse(jwtToken.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);

                return staffId;
            }
            catch
            {
                return null;
            }
        }
    }
}
