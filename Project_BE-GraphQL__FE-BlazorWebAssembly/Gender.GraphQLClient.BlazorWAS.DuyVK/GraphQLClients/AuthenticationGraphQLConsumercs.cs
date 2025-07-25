using Gender.GraphQLClient.BlazorWAS.DuyVK.ModelExtensions;
using Gender.GraphQLClient.BlazorWAS.DuyVK.Models;
using GraphQL.Client.Abstractions;

namespace Gender.GraphQLClient.BlazorWAS.DuyVK.GraphQLClients
{
    public class AuthenticationGraphQLConsumercs
    {
        // ===========================
        // === Fields
        // ===========================

        private readonly IGraphQLClient _graphQLClient;

        // ===========================
        // === Constructors
        // ===========================

        public AuthenticationGraphQLConsumercs(IGraphQLClient graphQLClient)
        {
            _graphQLClient = graphQLClient;
        }

        // ===========================
        // === Methods
        // ===========================

        // POST: Login
        public async Task<string?> LoginAsync(LoginRequest loginRequest)
        {
            var request = new GraphQL.GraphQLRequest
            {
                Query = @"mutation ($loginRequest: LoginRequestInput!) {
                    login(loginRequest: $loginRequest)
                }",
                Variables = new { loginRequest }
            };

            var response = await _graphQLClient.SendMutationAsync<LoginResponse>(request);
            return response.Data.login;
        }
    }
}
