using Gender.Repositories.DuyVK;
using Gender.Repositories.DuyVK.Models;
using Gender.Services.DuyVK.Interfaces;
using Microsoft.AspNetCore.Http;
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

        /// <summary>
        /// Get user account by username and password.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<SystemUserAccount?> GetUserAccountAsync(string username, string password)
        {
            return await _unitOfWork.SystemUserAccountRepository.GetAsync(username, password);
        }

        /// <summary>
        /// Gererate a JSON Web Token (JWT) for the given system user account.
        /// </summary>
        /// <param name="systemUserAccount"></param>
        /// <returns></returns>
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
                    expires: DateTime.Now.AddMinutes(_config.GetValue<double>("Jwt:ExpireInMinutes")),
                    signingCredentials: credentials
                );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenString;
        }

        /// <summary>
        /// ValidateJSONWebToken the JWT token and return the ClaimsPrincipal if valid, otherwise return null.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public ClaimsPrincipal? ValidateJSONWebToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]);

            var parametersForConfiguration = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = _config["Jwt:Issuer"],
                ValidateAudience = true,
                ValidAudience = _config["Jwt:Audience"],
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero // Disable the default 5 minute clock skew
            };

            try
            {
                var principal = tokenHandler.ValidateToken(token, parametersForConfiguration, out _);
                return principal;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Since SOAP does not support JWT authentication, this method is used to authorize the request
        /// </summary>
        /// <param name="accessor"></param>
        /// <param name="requiredRoles"></param>
        /// <returns></returns>
        public ClaimsPrincipal AuthorizeRequest(IHttpContextAccessor accessor, params string[] requiredRoles)
        {
            // Extract the bearer token with Authorization header
            var header = accessor.HttpContext?.Request.Headers["Authorization"].ToString();
            if (header == null || !header.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                throw new Exception("Missing Bearer token");
            }

            // Bearer axcerqcec, get the token part
            var token = header.Substring("Bearer".Length).Trim();

            // Validate the token
            var principal = ValidateJSONWebToken(token);
            if (principal == null)
            {
                throw new Exception("Invalid or expired token");
            }

            // Check requiredRoles
            if (requiredRoles.Length > 0)
            {
                var ok = requiredRoles.Any(r => principal.IsInRole(r));
                if (!ok)
                {
                    throw new Exception("Forbidden – insufficient role");
                }
            }

            return principal;
        }
    }
}
