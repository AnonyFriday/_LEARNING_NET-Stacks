using Gender.Services.DuyVK;
using Gender.SoapApiServices.DuyVK.SoapModelExtensions;
using System.ServiceModel;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Gender.SoapApiServices.DuyVK.SoapServices
{
    // =============================
    // === Interface Definition 
    // =============================

    [ServiceContract]
    public interface ISystemUserAccountSoapService
    {
        [OperationContract]
        public Task<LoginResponse> Login(LoginRequest request);
    }

    // =============================
    // === Service Implementation
    // =============================

    public class SystemUserAccountDuyVKSoapService : ISystemUserAccountSoapService
    {
        // =============================
        // === Interface Definition 
        // =============================

        private readonly IServiceProviders _service;
        private readonly IConfiguration _configuration;

        private static readonly JsonSerializerOptions _opts = new()
        {
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            PropertyNameCaseInsensitive = true
        };

        // =============================
        // === Constructors
        // =============================

        public SystemUserAccountDuyVKSoapService(IServiceProviders service, IConfiguration configuration)
        {
            _service = service;
            _configuration = configuration;
        }

        // =============================
        // === Methods
        // =============================

        // Login: Authenticate user and return login response
        public async Task<LoginResponse> Login(LoginRequest request)
        {
            // throw if username or password are empty
            if (string.IsNullOrWhiteSpace(request.UserName) || string.IsNullOrWhiteSpace(request.Password))
            {
                throw new FaultException("UserName and password must be provided.");
            }

            var user = await _service.UserAccountService.GetUserAccountAsync(request.UserName.Trim(), request.Password);

            // throw if username and password are invalid
            if (user is null)
            {
                throw new FaultException("Invalid username or password.");
            }

            string token = _service.UserAccountService.GenerateJSONWebToken(user);
            int ttlSecs = _configuration.GetValue<int>("Jwt:ExpireInMinutes") * 60;

            return new LoginResponse
            {
                Token = token,
                ExpiresIn = ttlSecs
            };
        }
    }
}
