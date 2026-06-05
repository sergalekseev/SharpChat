using SharpChat.Core.DataContracts;

namespace SharpChat.Server.Services;

public interface IAuthManager
{
    Task<AuthResponseDto> LoginAsync(LoginDto dto);
}

