using System.IdentityModel.Tokens.Jwt;
using System.Text;
using GroupChat.Models;
using GroupChat.Services;
using Microsoft.IdentityModel.Tokens;

namespace GroupChat.Test.Services;

[TestClass]
public class JwtAuthenticationServiceTest
{
    public JwtAuthenticationServiceTest()
    {

    }

    [TestInitialize]
    public void Initialize()
    {
       
    }

    [TestMethod]
    public void Authenticate_Returns_Valid_Token_For_User()
    {
        // Arrange
        var key = Encoding.ASCII.GetBytes("some_big_key_value_here_secret");
        var user = new User
        {
            Id = 1,
            Name = "John Doe"
        };
        var authService = new JwtAuthenticationService(key);

        // Act
        var token = authService.Authenticate(user);

        // Assert
        var tokenHandler = new JwtSecurityTokenHandler();
        var claimsPrincipal = tokenHandler.ValidateToken(token, new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false,
            ClockSkew = TimeSpan.Zero
        }, out SecurityToken validatedToken);

        Assert.IsNotNull(claimsPrincipal);
        Assert.AreEqual(user.Name, claimsPrincipal.Identity.Name);

        var idClaim = claimsPrincipal.FindFirst("Id");
        Assert.IsNotNull(idClaim);
        Assert.AreEqual(user.Id.ToString(), idClaim.Value);
    }
}
