using Blazored.LocalStorage;
using Gender.GraphQLClient.BlazorWAS.DuyVK.Auth;
using Gender.GraphQLClient.BlazorWAS.DuyVK.ModelExtensions;
using Gender.GraphQLClient.BlazorWAS.DuyVK.Models;
using GraphQL;
using GraphQL.Client.Abstractions;
using GraphQL.Client.Http;
using GraphQL.Transport;
using GraphQL.Validation;

namespace Gender.GraphQLClient.BlazorWAS.DuyVK.GraphQLClients
{
    public class MenstrualCycleReminderDuyVKGraphQLConsumer
    {
        // ===========================
        // === Fields
        // ===========================

        private readonly IGraphQLClient _graphQLClient;
        private readonly JwtAuthenticationStateProvider _storageProvider;

        // ===========================
        // === Constructors
        // ===========================

        public MenstrualCycleReminderDuyVKGraphQLConsumer(
            JwtAuthenticationStateProvider storageProvider,
            IGraphQLClient graphQLClient)
        {
            _graphQLClient = graphQLClient;
            _storageProvider = storageProvider;
        }

        // ===============================
        // === Get, Search
        // ===============================

        // GET: Get All with Pagination
        public async Task<PaginationResultResponse<List<MenstrualCycleReminderDuyVK>>> GetListAsync(SearchRequest searchRequest)
        {
            var request = new GraphQL.GraphQLRequest
            {
                Query = @"query ($searchRequest: SearchRequestInput!) {
                            menstrualCycleReminderDuyVKList(searchRequest: $searchRequest) {
                                totalItems
                                totalPages
                                currentPage
                                pageSize
                                items {
                                    menstrualCycleReminderDuyVKid
                                    reminderCategoryDuyVKid
                                    title
                                    note
                                    reminderDate
                                    sentAt
                                    isSent
                                    repeatInterval
                                    importanceScore
                                    createdAt
                                    updatedAt
                                }
                            }
                        }",
                Variables = new
                {
                    searchRequest = new SearchRequest
                    {
                        CurrentPage = searchRequest.CurrentPage,
                        PageSize = searchRequest.PageSize
                    }
                }
            };

            //var token = await _storageProvider.GetTokenFromStorage();



            var response = await _graphQLClient.SendQueryAsync<MenstrualCycleReminderDuyVKListResponse>(request);
            return response.Data.menstrualCycleReminderDuyVKList ?? new PaginationResultResponse<List<MenstrualCycleReminderDuyVK>>();
        }

        // GET: Get By Id
        public async Task<MenstrualCycleReminderDuyVK> GetByIdAsync(int id)
        {
            var request = new GraphQL.GraphQLRequest
            {
                Query = @"query ($id: Int!) {
                    menstrualCycleReminderDuyVKById(id: $id) {
                        menstrualCycleReminderDuyVKid
                        reminderCategoryDuyVKid
                        title
                        note
                        reminderDate
                        sentAt
                        isSent
                        repeatInterval
                        importanceScore
                        createdAt
                        updatedAt
                    }
                }",
                Variables = new { id }
            };

            var response = await _graphQLClient.SendQueryAsync<MenstrualCycleReminderDuyVKByIdResponse>(request);
            return response.Data.menstrualCycleReminderDuyVKById;
        }

        // GET: Search with Pagination
        public async Task<PaginationResultResponse<List<MenstrualCycleReminderDuyVK>>> SearchAsync(
            SearchMenstrualCycleReminderRequest searchRequest)
        {
            var request = new GraphQL.GraphQLRequest
            {
                Query = @"query ($searchRequest: SearchMenstrualCycleReminderRequestInput!) {
                    searchMenstrualCycleReminderDuyVKList(searchRequest: $searchRequest) {
                        totalItems
                        totalPages
                        currentPage
                        pageSize
                        items {
                            menstrualCycleReminderDuyVKid
                            reminderCategoryDuyVKid
                            title
                            note
                            reminderDate
                            sentAt
                            isSent
                            repeatInterval
                            importanceScore
                            createdAt
                            updatedAt
                        }
                    }
                }",
                Variables = new
                {
                    searchRequest = new SearchMenstrualCycleReminderRequest()
                    {
                        ColorCode = searchRequest.ColorCode,
                        CurrentPage = searchRequest.CurrentPage,
                        ImportantScore = searchRequest.ImportantScore,
                        PageSize = searchRequest.PageSize,
                        Title = searchRequest.Title
                    }
                }
            };

            var response = await _graphQLClient.SendQueryAsync<SearchMenstrualCycleReminderDuyVKListResponse>(request);
            return response.Data.searchMenstrualCycleReminderDuyVKList ?? new PaginationResultResponse<List<MenstrualCycleReminderDuyVK>>();
        }

        // ===============================
        // === Create, Update, Delete
        // ===============================

        // POST: Create reminder
        public async Task<bool> CreateAsync(MenstrualCycleReminderDuyVK model)
        {
            model.MenstrualCycleReminderDuyVKid = 0;
            model.CreatedAt = DateTime.UtcNow;
            model.UpdatedAt = DateTime.UtcNow;
            model.ReminderDate = DateTime.UtcNow;
            model.SentAt = DateTime.UtcNow;

            // add input for the model recieved
            var request = new GraphQL.GraphQLRequest
            {
                Query = @"mutation ($reminder: MenstrualCycleReminderDuyVKInput!) {
                    createMenstrualCycleReminderDuyVK(reminder: $reminder)
                }",
                Variables = new { reminder = model }
            };

            var response = await _graphQLClient.SendMutationAsync<CreateMenstrualCycleReminderDuyVKResponse>(request);
            return response.Data.createMenstrualCycleReminderDuyVK;
        }

        // POST: Update reminder
        public async Task<bool> UpdateAsync(MenstrualCycleReminderDuyVK model)
        {
            model.UpdatedAt = DateTime.UtcNow;

            var request = new GraphQL.GraphQLRequest
            {
                Query = @"mutation ($reminder: MenstrualCycleReminderDuyVKInput!) {
                    updateMenstrualCycleReminderDuyVK(reminder: $reminder)
                }",
                Variables = new { reminder = model }
            };

            var response = await _graphQLClient.SendMutationAsync<UpdateMenstrualCycleReminderDuyVKResponse>(request);
            return response.Data.updateMenstrualCycleReminderDuyVK;
        }

        // POST: Delete a reminder
        public async Task<bool> DeleteAsync(int id)
        {
            var request = new GraphQL.GraphQLRequest
            {
                Query = @"mutation ($id: Int!) {
                    deleteMenstrualCycleReminderDuyVK(id: $id)
                }",
                Variables = new { id }
            };

            var response = await _graphQLClient.SendMutationAsync<DeleteMenstrualCycleReminderDuyVKResponse>(request);
            return response.Data.deleteMenstrualCycleReminderDuyVK;
        }
    }
}
