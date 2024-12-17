using Microsoft.IdentityModel.Tokens;
using NLog;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace WizytowkaFirmy.Services
{
    public class JwtService
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        public string GenerujTokenJwt(string email, int wygasnieZa)
        {
            try
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("##############JwtServiceKey#####"));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var claims = new[]
                {
                new Claim(JwtRegisteredClaimNames.Sub, email),
                new Claim("role", "admin"),
                new Claim(JwtRegisteredClaimNames.Exp, DateTimeOffset.UtcNow.AddMinutes(wygasnieZa).ToUnixTimeSeconds().ToString())
            };

                var token = new JwtSecurityToken(
                    issuer: "twoja-firma",
                    audience: "twoja-firma",
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(wygasnieZa),
                    signingCredentials: credentials);

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return string.Empty;
            }
        }

        public bool WalidacjaTokenuJwt(string token)
        {
            try
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("##############JwtServiceKey#####"));
                var tokenHandler = new JwtSecurityTokenHandler();

                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "twoja-firma",
                    ValidAudience = "twoja-firma",
                    IssuerSigningKey = securityKey,
                    ClockSkew = TimeSpan.Zero // Eliminujemy tolerancję czasową
                }, out _);

                return true;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Weryfikacja tokenu JWT nie powiodła się.");
                return false;
            }
        }

    }
}
