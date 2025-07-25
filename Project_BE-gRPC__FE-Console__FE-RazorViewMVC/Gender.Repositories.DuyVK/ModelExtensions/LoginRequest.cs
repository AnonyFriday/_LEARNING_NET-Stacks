namespace Gender.Repositories.DuyVK.ModelExtensions
{
    public sealed record LoginRequest
    {
        public string UserName { get; init; }
        public string Password { get; init; }
    }
}
