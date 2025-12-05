using Toxiproxy.Net;
using Xunit;

namespace TSWMS.IntegrationTests;

public abstract class IntegrationTestBase : IAsyncLifetime
{
    protected readonly HttpClient OrderClient;
    private readonly Connection _proxyConnection;
    private const string ProductProxyName = "product_proxy";

    protected IntegrationTestBase()
    {
        OrderClient = new HttpClient
        {
            BaseAddress = new Uri("http://localhost:3200")
        };

        _proxyConnection = new Connection("localhost", 8474);
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync()
    {
        try
        {
            var client = _proxyConnection.Client();

            // 1. Get the proxy
            var proxy = await client.FindProxyAsync(ProductProxyName);

            // 2. Remove all Toxics
            // FIX: Use GetAllToxicsAsync() instead of accessing a .Toxics property
            var toxics = await proxy.GetAllToxicsAsync();

            foreach (var toxic in toxics)
            {
                await proxy.RemoveToxicAsync(toxic.Name);
            }

            // 3. Re-enable the proxy if it was disabled
            proxy.Enabled = true;
            await proxy.UpdateAsync();
        }
        catch
        {
            // Suppress cleanup errors
        }
    }

    protected async Task<Proxy> GetProductProxyAsync()
    {
        return await _proxyConnection.Client().FindProxyAsync(ProductProxyName);
    }
}