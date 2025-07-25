using Gender.GrpcService.DuyVK.Protos;
using Gender.Repositories.DuyVK.ModelExtensions;
using Gender.Services.DuyVK;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Gender.GrpcService.DuyVK.Services
{
    public class MenstrualCycleReminderDuyVKGRPCService : MenstrualCycleReminderDuyVKGRPC.MenstrualCycleReminderDuyVKGRPCBase
    {
        // =============================
        // === Fields
        // =============================

        private IServiceProviders _serviceProviders;

        // =============================
        // === Constructors
        // =============================

        public MenstrualCycleReminderDuyVKGRPCService(IServiceProviders serviceProviders)
        {
            _serviceProviders = serviceProviders;
        }

        // =============================
        // === Get + Search
        // =============================

        // GET: Get All with Paging
        [Authorize(Roles = "1,2")]
        public override async Task<MenstrualCycleReminderDuyVKPaginationResponse> GetAllPaged(Protos.SearchRequest request, ServerCallContext context)
        {
            var searchRequest = new Repositories.DuyVK.ModelExtensions.SearchRequest
            {
                PageSize = request.PageSize <= 0 ? 5 : request.PageSize,
                CurrentPage = request.CurrentPage <= 0 ? 1 : request.CurrentPage
            };

            var menstrualCycleReminderDuyVks = await _serviceProviders.MenstrualCycleReminderDuyVKService.GetAllAsync(searchRequest);

            // Using SerializerOptions to ignore cycles and trick the serialization
            var opt = new JsonSerializerOptions()
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            };

            var menstrualCycleReminderDuyVksJson = JsonSerializer.Serialize(menstrualCycleReminderDuyVks, opt);
            var metrualCycleReminderDuyVksList = JsonSerializer.Deserialize<PaginationResultResponse<List<MenstrualCycleReminderDuyVK>>>(menstrualCycleReminderDuyVksJson);

            // Add to list of response
            var resp = new MenstrualCycleReminderDuyVKPaginationResponse()
            {
                TotalItems = metrualCycleReminderDuyVksList.TotalItems,
                TotalPages = metrualCycleReminderDuyVksList.TotalPages,
                CurrentPage = metrualCycleReminderDuyVksList.CurrentPage,
                PageSize = metrualCycleReminderDuyVksList.PageSize,
                Items = { metrualCycleReminderDuyVksList.Items }
            };
            return resp;
        }


        // GET: Get All without Paging
        [Authorize(Roles = "1,2")]
        public override async Task<MenstrualCycleReminderDuyVKListResponse> GetAll(MenstrualCycleReminderDuyVKEmptyRequest request, ServerCallContext context)
        {
            var resp = new MenstrualCycleReminderDuyVKListResponse();
            var menstrualCycleReminderDuyVks = await _serviceProviders.MenstrualCycleReminderDuyVKService.GetAllAsync();

            // Guarding for null or empty list
            if (menstrualCycleReminderDuyVks.IsNullOrEmpty())
            {
                resp.Items.AddRange(new List<MenstrualCycleReminderDuyVK>());
                return resp;
            }

            // Using SerializerOptions to ignore cycles and trick the serialization
            var opt = new JsonSerializerOptions()
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            };

            var menstrualCycleReminderDuyVksJson = JsonSerializer.Serialize(menstrualCycleReminderDuyVks, opt);
            var metrualCycleReminderDuyVksList = JsonSerializer.Deserialize<List<MenstrualCycleReminderDuyVK>>(menstrualCycleReminderDuyVksJson);

            // Add to list of response
            resp.Items.AddRange(metrualCycleReminderDuyVksList);
            return resp;
        }

        // GET: Get By Id
        [Authorize(Roles = "1,2")]
        public override async Task<MenstrualCycleReminderDuyVK> GetById(MenstrualCycleReminderDuyVKIdRequest request, ServerCallContext context)
        {
            var menstrualCycleReminderDuyVk = await _serviceProviders.MenstrualCycleReminderDuyVKService.GetByIdAsync(request.MenstrualCycleReminderDuyVKid);

            // Guarding for null 
            if (menstrualCycleReminderDuyVk == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Item not found"));
            }

            // Using SerializerOptions to ignore cycles and trick the serialization
            var opt = new JsonSerializerOptions()
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            };

            var menstrualCycleReminderDuyVkJson = JsonSerializer.Serialize(menstrualCycleReminderDuyVk, opt);
            var metrualCycleReminderDuyVkValue = JsonSerializer.Deserialize<MenstrualCycleReminderDuyVK>(menstrualCycleReminderDuyVkJson);

            // return value
            return metrualCycleReminderDuyVkValue;
        }

        // GET: Search
        [Authorize(Roles = "1,2")]
        public override async Task<MenstrualCycleReminderDuyVKPaginationResponse> Search(MenstrualCycleReminderDuyVKSearchRequest request, ServerCallContext context)
        {
            var menstrualCycleReminderDuyVKSearchRequest = new MenstrualCycleReminderSearchRequest
            {
                PageSize = request.PageSize <= 0 ? 5 : request.PageSize,
                CurrentPage = request.CurrentPage <= 0 ? 1 : request.CurrentPage,
                ColorCode = request.ColorCode?.Trim(),
                ImportanceScore = request.ImportanceScore,
                Title = request.Title?.Trim()
            };

            var menstrualCycleReminderDuyVks = await _serviceProviders.MenstrualCycleReminderDuyVKService.SearchAsync(menstrualCycleReminderDuyVKSearchRequest);

            // Using SerializerOptions to ignore cycles and trick the serialization
            var opt = new JsonSerializerOptions()
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            };

            var menstrualCycleReminderDuyVksJson = JsonSerializer.Serialize(menstrualCycleReminderDuyVks, opt);
            var metrualCycleReminderDuyVksList = JsonSerializer.Deserialize<PaginationResultResponse<List<MenstrualCycleReminderDuyVK>>>(menstrualCycleReminderDuyVksJson);

            // Add to list of response
            var resp = new MenstrualCycleReminderDuyVKPaginationResponse()
            {
                TotalItems = metrualCycleReminderDuyVksList.TotalItems,
                TotalPages = metrualCycleReminderDuyVksList.TotalPages,
                CurrentPage = metrualCycleReminderDuyVksList.CurrentPage,
                PageSize = metrualCycleReminderDuyVksList.PageSize,
                Items = { metrualCycleReminderDuyVksList.Items }
            };
            return resp;
        }

        // =============================
        // === Create + Update + Delete
        // =============================

        // CREATE: create a new reminder
        [Authorize(Roles = "1,2")]
        public override async Task<MutationResultResponse> Create(MenstrualCycleReminderDuyVK request, ServerCallContext context)
        {
            try
            {
                // Validate reminder category fiedl
                if (request.ReminderCategoryDuyVKid <= 0)
                    throw new RpcException(new Status(
                        StatusCode.InvalidArgument,
                        "ReminderCategoryDuyVKId must be > 0"));

                var entity = new Repositories.DuyVK.Models.MenstrualCycleReminderDuyVK()
                {
                    MenstrualCycleReminderDuyVKid = 0,
                    ReminderCategoryDuyVKid = request.ReminderCategoryDuyVKid,
                    Title = request.Title,
                    Note = request.Note,
                    ReminderDate = DateTime.Parse(request.ReminderDate),
                    SentAt = string.IsNullOrEmpty(request.SentAt) ? null : DateTime.Parse(request.SentAt),
                    IsSent = request.IsSent,
                    RepeatInterval = request.RepeatInterval,
                    ImportanceScore = request.ImportanceScore,
                    CreatedAt = DateTime.Parse(request.CreatedAt),
                    UpdatedAt = DateTime.Parse(request.UpdatedAt),
                };

                // call create service
                bool isSuccessful = await _serviceProviders.MenstrualCycleReminderDuyVKService.CreateAsync(entity);

                return new MutationResultResponse
                {
                    IsSuccessful = isSuccessful,
                };
            }
            catch (RpcException)
            {
                // re-throw known RpcExceptions verbatim
                throw;
            }
            catch (Exception ex)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Create failed. Please try again"));
            }
        }

        // DELETE: delete a reminder by Id
        [Authorize(Roles = "1")]
        public override async Task<MutationResultResponse> Delete(MenstrualCycleReminderDuyVKIdRequest request, ServerCallContext context)
        {
            try
            {
                if (request.MenstrualCycleReminderDuyVKid <= 0)
                    throw new RpcException(new Status(
                        StatusCode.InvalidArgument,
                        "MenstrualCycleReminderDuyVKId must be a positive integer"));

                bool isSuccessful = await _serviceProviders
                    .MenstrualCycleReminderDuyVKService
                    .DeleteAsync(request.MenstrualCycleReminderDuyVKid);

                return new MutationResultResponse
                {
                    IsSuccessful = isSuccessful
                };
            }
            catch (RpcException)
            {
                // re-throw known RpcExceptions verbatim
                throw;
            }
            catch (Exception)
            {
                // wrap any other failure
                throw new RpcException(new Status(
                    StatusCode.Internal,
                    "Delete failed. Please try again"));
            }
        }

        // UPDATE: update an existing reminder
        [Authorize(Roles = "1,2")]
        public override async Task<MutationResultResponse> Update(MenstrualCycleReminderDuyVK request, ServerCallContext context)
        {
            try
            {
                // Validate “Id” must be present
                if (request.MenstrualCycleReminderDuyVKid <= 0)
                    throw new RpcException(new Status(
                        StatusCode.InvalidArgument,
                        "MenstrualCycleReminderDuyVKId must be a positive integer"));

                // Map to the EF-Core entity
                var entity = new Repositories.DuyVK.Models.MenstrualCycleReminderDuyVK()
                {
                    MenstrualCycleReminderDuyVKid = request.MenstrualCycleReminderDuyVKid,
                    ReminderCategoryDuyVKid = request.ReminderCategoryDuyVKid,
                    Title = request.Title,
                    Note = request.Note,
                    ReminderDate = DateTime.Parse(request.ReminderDate),
                    SentAt = string.IsNullOrEmpty(request.SentAt) ? null : DateTime.Parse(request.SentAt),
                    IsSent = request.IsSent,
                    RepeatInterval = request.RepeatInterval,
                    ImportanceScore = request.ImportanceScore,
                    CreatedAt = string.IsNullOrEmpty(request.CreatedAt)
                                               ? (DateTime?)null
                                               : DateTime.Parse(request.CreatedAt),
                    UpdatedAt = string.IsNullOrEmpty(request.UpdatedAt)
                                               ? (DateTime?)null
                                               : DateTime.Parse(request.UpdatedAt),
                };

                // Call the service
                bool isSuccessful = await _serviceProviders
                    .MenstrualCycleReminderDuyVKService
                    .UpdateAsync(entity);

                // Return result
                return new MutationResultResponse
                {
                    IsSuccessful = isSuccessful
                };
            }
            catch (RpcException)
            {
                throw;
            }
            catch (Exception)
            {
                throw new RpcException(new Status(
                    StatusCode.Internal,
                    "Update failed. Please try again"));
            }
        }


    }
}
