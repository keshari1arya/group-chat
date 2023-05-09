using GroupChat.Models;

namespace GroupChat.Services;
public interface IJwtAuthenticationService
{
    string Authenticate(User user);
}
