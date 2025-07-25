using Gender.Repositories.DuyVK;
using Gender.Repositories.DuyVK.Models;
using Gender.Services.DuyVK.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Gender.Services.DuyVK
{
    public class SystemUserAccountService : ISystemUserAccountService
    {
        // =============================
        // === Fields
        // =============================

        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _config;

        // =============================
        // === Constructors
        // =============================

        public SystemUserAccountService(IUnitOfWork unitOfWork, IConfiguration config)
        {
            _unitOfWork = unitOfWork;
            _config = config;
        }

        // =============================
        // === Methods
        // =============================

        public async Task<SystemUserAccount?> GetUserAccountAsync(string username, string password)
        {
            return await _unitOfWork.SystemUserAccountRepository.GetAsync(username, password);
        }

        public string GenerateJSONWebToken(SystemUserAccount systemUserAccount)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, systemUserAccount.UserName),
                new Claim(ClaimTypes.Email, systemUserAccount.Email),
                new Claim(ClaimTypes.Role, systemUserAccount.RoleId.ToString())
            };

            // Generate the token that include
            var token = new JwtSecurityToken(
                    issuer: _config["Jwt:Issuer"],
                    audience: _config["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(_config.GetValue<double>("Jwt:ExpirationInMinutes")),
                    signingCredentials: credentials
                );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenString;
        }
    }
}
