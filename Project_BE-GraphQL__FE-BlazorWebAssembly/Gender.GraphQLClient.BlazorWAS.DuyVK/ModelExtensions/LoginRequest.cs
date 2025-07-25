namespace Gender.GraphQLClient.BlazorWAS.DuyVK.ModelExtensions
{
    public sealed record LoginRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
