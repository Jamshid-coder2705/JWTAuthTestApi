using Application.Services.Authentication;
using Blazored.LocalStorage;
using BlazorWebApi.States;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace BlazorWebApi
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");
            builder.Services.AddCascadingAuthenticationState();
            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7260/") });
            builder.Services.AddScoped<IAccount, AccountService>();
            builder.Services.AddBlazoredLocalStorage();
            builder.Services.AddAuthorizationCore();

            builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
            await builder.Build().RunAsync();
        }
    }
}
