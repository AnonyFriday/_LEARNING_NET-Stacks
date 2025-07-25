using Gender.GrpcService.DuyVK.Protos;
using Gender.Services.DuyVK;
using Grpc.Core;

namespace Gender.GrpcService.DuyVK.Services
{
    public class AuthDuyVKGRPCService : AuthDuyVKGRPC.AuthDuyVKGRPCBase
    {
        // =============================
        // === Fields
        // =============================

        private IServiceProviders _serviceProviders;
        private IConfiguration _configuration;

        // =============================
        // === Constructors
        // =============================
        public AuthDuyVKGRPCService(IServiceProviders serviceProviders, IConfiguration configuration)
        {
            _serviceProviders = serviceProviders;
            _configuration = configuration;
        }

        // =============================
        // === Methods
        // =============================

        // LOGIN: login with username and password
        public override async Task<LoginDuyVKResponse> Login(LoginDuyVKRequest request, ServerCallContext context)
        {
            // get user
            var user = await _serviceProviders.UserAccountService.GetUserAccountAsync(request.Username, request.Password);
            if (user == null) return null;

            // generate token
            var token = _serviceProviders.UserAccountService.GenerateJSONWebToken(user);
            if (string.IsNullOrEmpty(token)) return null;

            var resp = new LoginDuyVKResponse
            {
                Token = token,
                ExpiresIn = _configuration.GetSection("Jwt").GetValue<int>("ExpireInMinutes") * 60
            };

            return resp;
        }
    }
}
