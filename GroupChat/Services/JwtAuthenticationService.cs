
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using GroupChat.Models;
using Microsoft.IdentityModel.Tokens;

namespace GroupChat.Services;
public class JwtAuthenticationService : IJwtAuthenticationService
{
    private readonly byte[] _key;

    public JwtAuthenticationService(byte[] key)
    {
        _key = key;
    }

    public string Authenticate(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim("Id", user.Id.ToString()),
            }),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(_key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
