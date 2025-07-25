using Gender.GraphQLClient.BlazorWAS.DuyVK.ModelExtensions;
using Gender.GraphQLClient.BlazorWAS.DuyVK.Models;
using GraphQL;
using GraphQL.Client.Abstractions;

namespace Gender.GraphQLClient.BlazorWAS.DuyVK.GraphQLClients
{
    public class ReminderCategoryDuyVKGraphQLConsumer
    {
        // ===========================
        // === Fields
        // ===========================

        private readonly IGraphQLClient _graphQLClient;

        // ===========================
        // === Constructors
        // ===========================

        public ReminderCategoryDuyVKGraphQLConsumer(IGraphQLClient graphQLClient) => _graphQLClient = graphQLClient;

        // ===============================
        // === Get, Search
        // ===============================

        // GET: Get List
        public async Task<PaginationResultResponse<List<ReminderCategoryDuyVK>>> GetListAsync(SearchRequest searchRequest)
        {
            try
            {
                var request = new GraphQLRequest
                {
                    Query = @"
                        query ($searchRequest: SearchRequest) {
                            reminderCategoryDuyVKList(searchRequest: $searchRequest) {
                                totalItems
                                totalPages
                                currentPage
                                pageSize
                                items {
                                    reminderCategoryDuyVKid
                                    code
                                    name
                                    description
                                    isActive
                                    priorityLevel
                                    defaultOffset
                                    colorCode
                                    createdAt
                                    updatedAt
                                }
                            }
                        }
                    ",
                    Variables = new { searchRequest }
                };

                var response = await _graphQLClient.SendQueryAsync<ReminderCategoryDuyVKListResponse>(request);
                return response.Data?.reminderCategoryDuyVKList ?? new PaginationResultResponse<List<ReminderCategoryDuyVK>>();
            }
            catch
            {
                return new PaginationResultResponse<List<ReminderCategoryDuyVK>>();
            }
        }

        // GET: Get by Id
        public async Task<ReminderCategoryDuyVK> GetByIdAsync(int id)
        {
            try
            {
                var request = new GraphQLRequest
                {
                    Query = @"
                        query ($id: Int!) {
                            reminderCategoryDuyVKById(id: $id) {
                                reminderCategoryDuyVKid
                                code
                                name
                                description
                                isActive
                                priorityLevel
                                defaultOffset
                                colorCode
                                createdAt
                                updatedAt
                            }
                        }
                    ",
                    Variables = new { id }
                };

                var response = await _graphQLClient.SendQueryAsync<ReminderCategoryDuyVKByIdResponse>(request);
                return response.Data?.reminderCategoryDuyVKById ?? new ReminderCategoryDuyVK();
            }
            catch
            {
                return new ReminderCategoryDuyVK();
            }
        }

        // GET: Get all
        public async Task<List<ReminderCategoryDuyVK>> GetReminderCategoryDuyVKListAll()
        {
            try
            {
                var request = new GraphQLRequest
                {
                    Query = @"
                                query ReminderCategoryDuyVKListAll {
                                    reminderCategoryDuyVKListAll {
                                        reminderCategoryDuyVKid
                                        code
                                        name
                                        description
                                        isActive
                                        priorityLevel
                                        defaultOffset
                                        colorCode
                                        createdAt
                                        updatedAt
                                    }
                                }
                            "
                };

                var response = await _graphQLClient.SendQueryAsync<ReminderCategoryDuyVKListAllResponse>(request);
                return response.Data?.reminderCategoryDuyVKListAll ?? new List<ReminderCategoryDuyVK>();
            }
            catch (Exception ex)
            {
                // Log exception if necessary
                return new List<ReminderCategoryDuyVK>();
            }
        }


        // ===============================
        // === Create, Update, Delete
        // ===============================

        public async Task<int> CreateAsync(ReminderCategoryDuyVK reminderCategory)
        {
            try
            {
                reminderCategory.ReminderCategoryDuyVKid = 0;
                reminderCategory.CreatedAt = DateTime.UtcNow;
                reminderCategory.UpdatedAt = DateTime.UtcNow;

                var request = new GraphQLRequest
                {
                    Query = @"
                        mutation ($input: ReminderCategoryDuyVKInput!) {
                            createReminderCategoryDuyVK(reminderCategoryDuyVK: $input)
                        }
                    ",
                    Variables = new { input = reminderCategory }
                };

                var response = await _graphQLClient.SendMutationAsync<CreateReminderCategoryDuyVKResponse>(request);
                return response.Data?.createReminderCategoryDuyVK ?? 0;
            }
            catch
            {
                return 0;
            }
        }

        public async Task<int> UpdateAsync(ReminderCategoryDuyVK reminderCategory)
        {
            try
            {
                reminderCategory.UpdatedAt = DateTime.UtcNow;

                var request = new GraphQLRequest
                {
                    Query = @"
                        mutation ($input: ReminderCategoryDuyVKInput!) {
                            updateReminderCategoryDuyVK(reminderCategoryDuyVK: $input)
                        }
                    ",
                    Variables = new { input = reminderCategory }
                };

                var response = await _graphQLClient.SendMutationAsync<UpdateReminderCategoryDuyVKResponse>(request);
                return response.Data?.updateReminderCategoryDuyVK ?? 0;
            }
            catch
            {
                return 0;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var request = new GraphQLRequest
                {
                    Query = @"
                        mutation ($id: Int!) {
                            deleteReminderCategoryDuyVK(id: $id)
                        }
                    ",
                    Variables = new { id }
                };

                var response = await _graphQLClient.SendMutationAsync<DeleteReminderCategoryDuyVKResponse>(request);
                return response.Data?.deleteReminderCategoryDuyVK ?? false;
            }
            catch
            {
                return false;
            }
        }
    }
}
