
using Gender.Services.DuyVK;
using Gender.SoapApiServices.DuyVK.SoapModels;
using Microsoft.AspNetCore.Authorization;
using System.ServiceModel;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Gender.SoapApiServices.DuyVK.SoapServices
{
    // =============================
    // === Interface Definition 
    // =============================

    [ServiceContract]
    public interface IReminderCategoryDuyVKSoapService
    {
        [OperationContract]
        Task<List<ReminderCategoryDuyVK>> GetAllAsync();
    }

    // =============================
    // === Service Implementation
    // =============================

    public class ReminderCategoryDuyVKSoapService : IReminderCategoryDuyVKSoapService
    {
        // =============================
        // === Interface Definition 
        // =============================

        private readonly IServiceProviders _service;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private static readonly JsonSerializerOptions _serializerOptions = new()
        {
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            PropertyNameCaseInsensitive = true
        };

        // =============================
        // === Constructors
        // =============================

        public ReminderCategoryDuyVKSoapService(IServiceProviders service, IHttpContextAccessor httpContextAccessor)
        {
            _service = service;
            _httpContextAccessor = httpContextAccessor;
        }

        // =============================
        // === Methods
        // =============================

        // GET: Get all reminder categories
        public async Task<List<ReminderCategoryDuyVK>> GetAllAsync()
        {
            try
            {
                // Authorize the request
                _service.UserAccountService.AuthorizeRequest(_httpContextAccessor, "1", "2");

                var rawData = await _service.ReminderCategoryDuyVKService.GetAllAsync();
                var json = JsonSerializer.Serialize(rawData, _serializerOptions);
                return JsonSerializer.Deserialize<List<ReminderCategoryDuyVK>>(json, _serializerOptions);
            }
            catch (Exception ex)
            {
                throw new FaultException($"Error: {ex.Message}");
            }
        }
    }
}
