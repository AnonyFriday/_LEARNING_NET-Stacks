using Gender.Services.DuyVK;
using Gender.SoapApiServices.DuyVK.SoapModelExtensions;
using Gender.SoapApiServices.DuyVK.SoapModels;
using System.ServiceModel;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Gender.SoapApiServices.DuyVK.SoapServices
{
    // =============================
    // === Interface Definition 
    // =============================

    [ServiceContract]
    public interface IMenstrualCycleReminderDuyVKSoapService
    {
        [OperationContract]
        Task<List<MenstrualCycleReminderDuyVK>> GetAll();

        [OperationContract]
        Task<PaginationResultResponse<List<MenstrualCycleReminderDuyVK>>> GetAllPaged(SearchRequest searchRequest);

        [OperationContract]
        Task<MenstrualCycleReminderDuyVK> GetById(int id);

        [OperationContract]
        Task<PaginationResultResponse<List<MenstrualCycleReminderDuyVK>>> Search(MenstrualCycleReminderSearchRequest searchMenstrualCycleReminderRequest);

        [OperationContract]
        Task<int> Create(MenstrualCycleReminderDuyVK model);

        [OperationContract]
        Task<int> Update(MenstrualCycleReminderDuyVK model);

        [OperationContract]
        Task<int> Delete(int id);
    }

    // =============================
    // === Service Implementation
    // =============================

    public class MenstrualCycleReminderDuyVKSoapService : IMenstrualCycleReminderDuyVKSoapService
    {
        // =============================
        // === Fields
        // =============================

        private readonly IServiceProviders _service;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private static readonly JsonSerializerOptions _serializerOptions = new()
        {
            PropertyNameCaseInsensitive = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
        };

        // =============================
        // === Constructors
        // =============================

        public MenstrualCycleReminderDuyVKSoapService(
            IServiceProviders service, IHttpContextAccessor httpContextAccessor)
        {
            _service = service;
            _httpContextAccessor = httpContextAccessor;
        }

        // =============================
        // === Gets & Search
        // =============================

        // GET: Get Menstrual Cycle Reminders
        public async Task<PaginationResultResponse<List<MenstrualCycleReminderDuyVK>>> Search(MenstrualCycleReminderSearchRequest searchRequest)
        {
            try
            {
                // Authorize the request
                _service.UserAccountService.AuthorizeRequest(_httpContextAccessor, "1", "2");

                // results
                var json = JsonSerializer.Serialize(searchRequest, _serializerOptions);
                var repoRequest = JsonSerializer.Deserialize<Repositories.DuyVK.ModelExtensions.MenstrualCycleReminderSearchRequest>(json, _serializerOptions);

                var repoResult = await _service.MenstrualCycleReminderDuyVKService.SearchAsync(repoRequest);

                var resultJson = JsonSerializer.Serialize(repoResult, _serializerOptions);
                return JsonSerializer.Deserialize<PaginationResultResponse<List<MenstrualCycleReminderDuyVK>>>(resultJson, _serializerOptions);
            }
            catch (Exception ex)
            {
                throw new FaultException($"Error: {ex.Message}");
            }
        }

        // GET: Search Menstrual Cycle Reminders with pagination
        public async Task<PaginationResultResponse<List<MenstrualCycleReminderDuyVK>>> GetAllPaged(SearchRequest searchRequest)
        {
            try
            {
                // Authorize the request
                _service.UserAccountService.AuthorizeRequest(_httpContextAccessor, "1", "2");

                var json = JsonSerializer.Serialize(searchRequest, _serializerOptions);
                var repoRequest = JsonSerializer.Deserialize<Repositories.DuyVK.ModelExtensions.SearchRequest>(json, _serializerOptions);

                var repoResult = await _service.MenstrualCycleReminderDuyVKService.GetAllAsync(repoRequest);

                var resultJson = JsonSerializer.Serialize(repoResult, _serializerOptions);
                return JsonSerializer.Deserialize<PaginationResultResponse<List<MenstrualCycleReminderDuyVK>>>(resultJson, _serializerOptions);
            }
            catch (Exception ex)
            {
                throw new FaultException($"Error: {ex.Message}");
            }
        }

        // GET: Get all Menstrual Cycle Reminders with pagination
        public async Task<List<MenstrualCycleReminderDuyVK>> GetAll()
        {
            try
            {
                // Authorize the request
                _service.UserAccountService.AuthorizeRequest(_httpContextAccessor, "1", "2");

                var data = await _service.MenstrualCycleReminderDuyVKService.GetAllAsync();
                var json = JsonSerializer.Serialize(data, _serializerOptions);
                return JsonSerializer.Deserialize<List<MenstrualCycleReminderDuyVK>>(json, _serializerOptions);
            }
            catch (Exception ex)
            {
                throw new FaultException($"Error: {ex.Message}");
            }
        }

        // GET: Get all Menstrual Cycle Reminders with paginationq
        public async Task<MenstrualCycleReminderDuyVK> GetById(int id)
        {
            try
            {
                // Authorize the request
                _service.UserAccountService.AuthorizeRequest(_httpContextAccessor, "1", "2");

                var entity = await _service.MenstrualCycleReminderDuyVKService.GetByIdAsync(id);
                var json = JsonSerializer.Serialize(entity, _serializerOptions);
                return JsonSerializer.Deserialize<MenstrualCycleReminderDuyVK>(json, _serializerOptions);
            }
            catch (Exception ex)
            {
                throw new FaultException($"Error: {ex.Message}");
            }
        }

        // =============================
        // === Create, Update, Delete
        // =============================

        // Create new Menstrual Cycle Reminder
        public async Task<int> Create(MenstrualCycleReminderDuyVK model)
        {
            try
            {
                // Authorize the request
                _service.UserAccountService.AuthorizeRequest(_httpContextAccessor, "1", "2");

                var json = JsonSerializer.Serialize(model, _serializerOptions);
                var entity = JsonSerializer.Deserialize<Repositories.DuyVK.Models.MenstrualCycleReminderDuyVK>(json, _serializerOptions);

                var result = await _service.MenstrualCycleReminderDuyVKService.CreateAsync(entity);
                return result ? 1 : 0;
            }
            catch (Exception ex)
            {
                throw new FaultException($"Error: {ex.Message}");
            }
        }

        // Update: Update existing Menstrual Cycle Reminder
        public async Task<int> Update(MenstrualCycleReminderDuyVK model)
        {
            try
            {
                // Authorize the request
                _service.UserAccountService.AuthorizeRequest(_httpContextAccessor, "1", "2");

                var json = JsonSerializer.Serialize(model, _serializerOptions);
                var entity = JsonSerializer.Deserialize<Repositories.DuyVK.Models.MenstrualCycleReminderDuyVK>(json, _serializerOptions);

                var result = await _service.MenstrualCycleReminderDuyVKService.UpdateAsync(entity);
                return result ? 1 : 0;
            }
            catch (Exception ex)
            {
                throw new FaultException($"Error: {ex.Message}");
            }
        }

        // Delete: Delete Menstrual Cycle Reminder by ID
        public async Task<int> Delete(int id)
        {
            try
            {
                // Authorize the request
                _service.UserAccountService.AuthorizeRequest(_httpContextAccessor, "1");

                var result = await _service.MenstrualCycleReminderDuyVKService.DeleteAsync(id);
                return result ? 1 : 0;
            }
            catch (Exception ex)
            {
                throw new FaultException($"Error: {ex.Message}");
            }
        }
    }
}
