namespace SharpChat.Core.DataContracts;

public class AuthResponseDto
{
    public bool Success { get; set; } = false;
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime AccessTokenExpiry { get; set; }
    public DateTime RefreshTokenExpiry { get; set; }
}

