using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration.Memory;

namespace UserManagement.Api.IntegrationTests.Core;

public class ContainerTestBase(ContainerEnvironment containerEnvironment)
    : WebApplicationFactory<Program>, IClassFixture<ContainerEnvironment>
{
    protected ContainerEnvironment Environment => containerEnvironment;

    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureHostConfiguration(
            config =>
            {
                builder.UseEnvironment("Development");

                ConfigurationBuilder configurationBuilder = new();

                Dictionary<string, string> configCollection = new()
                {
                    { "ConnectionStrings:DefaultConnection", Environment.MsSqlContainer.GetConnectionString() },
                };

                configurationBuilder.AddInMemoryCollection(configCollection!);

                config.Sources.Add(
                    new MemoryConfigurationSource()
                    {
                        InitialData = configCollection!,
                    });

                config.AddConfiguration(configurationBuilder.Build());
            });

        return base.CreateHost(builder);
    }
}
