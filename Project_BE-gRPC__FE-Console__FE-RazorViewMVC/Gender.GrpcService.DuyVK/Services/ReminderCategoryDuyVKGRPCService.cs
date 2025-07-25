using Gender.GrpcService.DuyVK.Protos;
using Gender.Services.DuyVK;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Gender.GrpcService.DuyVK.Services
{
    [Authorize(Roles = "1,2")]
    public class ReminderCategoryDuyVKGRPCService : ReminderCategoryDuyVKGRPC.ReminderCategoryDuyVKGRPCBase
    {
        // =============================
        // === Fields
        // =============================

        private IServiceProviders _serviceProviders;

        // =============================
        // === Constructors
        // =============================

        public ReminderCategoryDuyVKGRPCService(IServiceProviders serviceProviders)
        {
            _serviceProviders = serviceProviders;
        }

        // =============================
        // === Methods
        // =============================

        public override async Task<ReminderCategoryDuyVKListResponse> GetAll(ReminderCategoryDuyVKEmptyRequest request, ServerCallContext context)
        {
            var reminderCategoryDuyVKs = await _serviceProviders.ReminderCategoryDuyVKService.GetAllAsync();
            var resp = new ReminderCategoryDuyVKListResponse();

            // Guarding for null or empty list
            if (reminderCategoryDuyVKs.IsNullOrEmpty())
            {
                resp.Items.AddRange(new List<ReminderCategoryDuyVK>());
                return resp;
            }

            // Using SerializerOptions to ignore cycles and trick the serialization
            var opt = new JsonSerializerOptions()
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            };

            var reminderCategoryDuyVKsJson = JsonSerializer.Serialize(reminderCategoryDuyVKs, opt);
            var reminderCategoryDuyVKsList = JsonSerializer.Deserialize<List<ReminderCategoryDuyVK>>(reminderCategoryDuyVKsJson);

            // Add to list of response
            resp.Items.AddRange(reminderCategoryDuyVKsList);
            return resp;
        }
    }
}
