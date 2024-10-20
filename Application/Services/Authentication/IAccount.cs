using Application.DTOs;

namespace Application.Services.Authentication
{
    public interface IAccount
    {
        Task<RegistationRespons> RegisterAccountAsync(RegisterUserDTO model);
        Task<LoginRespons> LogInAccountAsync(LoginDTO model);

    }
}
