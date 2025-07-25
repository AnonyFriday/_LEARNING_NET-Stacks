
using Gender.GrpcClient.DuyVK;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var app = new App();
        await app.RunAsync();
    }
}