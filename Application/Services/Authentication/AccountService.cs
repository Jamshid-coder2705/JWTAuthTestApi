using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Authentication
{
    public class AccountService : IAccount
    {
        private readonly HttpClient httpClient;

        public AccountService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        public async Task<LoginRespons> LogInAccountAsync(LoginDTO model)
        {
            var respons = await httpClient.PostAsJsonAsync("api/user/login",model);

            var result = await respons.Content.ReadFromJsonAsync<LoginRespons>();

            return result!;
        }

        public async Task<RegistationRespons> RegisterAccountAsync(RegisterUserDTO model)
        {
            var respons = await httpClient.PostAsJsonAsync("api/user/register", model);

            var result = await respons.Content.ReadFromJsonAsync<RegistationRespons>();

            return result!;
        }
    }
}
