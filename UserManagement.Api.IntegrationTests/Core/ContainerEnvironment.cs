using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using DotNet.Testcontainers.Networks;
using Testcontainers.MsSql;

namespace UserManagement.Api.IntegrationTests.Core;

public class ContainerEnvironment : IAsyncLifetime
{
    private readonly HttpClient _httpClient = new();
    private readonly INetwork _network;

    public MsSqlContainer MsSqlContainer { get;  }

    public List<IContainer> Containers { get; } = [];

    public ContainerEnvironmentConfiguration Configuration { get; }

    public ContainerEnvironment()
    {
        _network = new NetworkBuilder()
            .Build();

        MsSqlContainer = new MsSqlBuilder()
            .WithNetwork(_network)
            .Build();

        Containers.Add(MsSqlContainer);

        IConfigurationRoot config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true)
            .Build();

        Configuration = config.Get<ContainerEnvironmentConfiguration>()!;
    }

    public async Task InitializeAsync()
    {
        await _network
            .CreateAsync()
            .ConfigureAwait(false);

        foreach (IContainer container in Containers)
        {
            await container.StartAsync().ConfigureAwait(false);
        }
    }

    public async Task DisposeAsync()
    {
        foreach (IContainer container in Containers)
        {
            await container.StopAsync().ConfigureAwait(false);
        }

        await _network
            .DeleteAsync()
            .ConfigureAwait(false);

        _httpClient.Dispose();
    }
}
