using Microsoft.IdentityModel.Tokens;
using SharpChat.Core.DataContracts;
using SharpChat.Core.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SharpChat.Server.Services;

public class AuthManager : IAuthManager
{
    private readonly IConfiguration _config;

    private static readonly Dictionary<string, User> _users = new Dictionary<string, User>()
    {
        { "Test1", new User{ Id = 0, Username = "Test1"  } },
        { "Test2", new User{ Id = 1, Username = "Test2"  } },
        { "Test3", new User{ Id = 2, Username = "Test3"  } },
    };

    public AuthManager(IConfiguration config)
    {
        _config = config;
    }

    public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
    {
        if (!_users.ContainsKey(dto.Username) || dto.Password != "Test")
        {
            throw new UnauthorizedAccessException("Invalid credentials");
        }

        var accessToken = GenerateJwtToken(_users[dto.Username]);
        var refreshToken = GenerateRefreshToken();

        // TODO: Save refresh token to database linked to user

        return new AuthResponseDto
        {
            Success = true,
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            AccessTokenExpiry = DateTime.UtcNow.AddMinutes(
                _config.GetValue<int>("Jwt:AccessTokenExpirationMinutes")),
            RefreshTokenExpiry = DateTime.UtcNow.AddDays(
                _config.GetValue<int>("Jwt:RefreshTokenExpirationDays"))
        };
    }

    private string GenerateJwtToken(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_config.GetValue<int>("Jwt:AccessTokenExpirationMinutes")),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private string GenerateRefreshToken() => Guid.NewGuid().ToString();
}

