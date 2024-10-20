using Application.Contracts;
using Application.DTOs;
using Domain.Entities;
using Infrastracture.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastracture.Repo
{
    public class UserRepo : IUser
    {
        private readonly AppDbContext appDbContext;
        private readonly IConfiguration configuration;
        public UserRepo(AppDbContext appDbContext,IConfiguration configuration)
        {
            this.appDbContext = appDbContext;
            this.configuration = configuration;
        }


        public async Task<LoginRespons> LoginUserAsync(LoginDTO loginDTO)
        {
            var getUser = await FindUserByEmail(loginDTO.Email!);

            if (getUser == null) return new LoginRespons(false,"user not found");

            bool checkPassword = BCrypt.Net.BCrypt.Verify(loginDTO.Password,getUser.Password);
            if (checkPassword)
            {
                return new LoginRespons(true, "Login successfully", GenerateJWTToken(getUser));
            }
            else
            {
                return new LoginRespons(false, "Invalid");
            }
            
        }



        private string GenerateJWTToken(ApplicationUser user)
        {
            var securityKay = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!));
            var credentail = new SigningCredentials(securityKay, SecurityAlgorithms.HmacSha256);
            var userClaims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name!),
                new Claim(ClaimTypes.Email, user.Email!)
            };
            var token = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                claims: userClaims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentail
                );
            return new JwtSecurityTokenHandler().WriteToken(token);

        }



        private async Task<ApplicationUser>  FindUserByEmail(string email) =>
            await appDbContext.Users.FirstOrDefaultAsync(u => u.Email == email);


        public async Task<RegistationRespons> RegisterUserAsync(RegisterUserDTO registerUserDTO)
        {
            var getUser = await FindUserByEmail(registerUserDTO.Email!);
            if (getUser != null)
            {
                return new RegistationRespons(false, "User allready");
            }

            appDbContext.Users.Add(new ApplicationUser()
            {
                Name = registerUserDTO.Name,
                Email = registerUserDTO.Email!,
                Password = BCrypt.Net.BCrypt.HashPassword(registerUserDTO.Password)
            });

            await appDbContext.SaveChangesAsync();
            return new RegistationRespons(true, " Registration complited");
        }
    }
}
