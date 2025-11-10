using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using UserManagement.Data.Patterns;

namespace UserManagement.Data.Tests;

public class TestModel
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
}

public class EntityRepositoryTests : IDisposable
{
    private DbContextOptions _dbContextOptions;
    private SqliteConnection _connection;

    private readonly TestDbContext<TestModel> _testDbContext;
    private readonly IEntityRepository<TestModel> _testEntityRepository;

    public EntityRepositoryTests()
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();

        _dbContextOptions = new DbContextOptionsBuilder()
            .UseSqlite(_connection)
            .Options;

        _testDbContext = new TestDbContext<TestModel>(_dbContextOptions);
        _testDbContext.Database.EnsureDeleted();
        _testDbContext.Database.EnsureCreated();

        _testDbContext.AddRange(
            new TestModel
            {
                Id = 0,
                Name = "Test1"
            },
            new TestModel
            {
                Id = 0,
                Name = "Test2"
            });

        _testDbContext.SaveChanges();

        _testEntityRepository = new EntityRepository<TestModel>(_testDbContext);
    }

    public void Dispose()
    {
        _connection.Dispose();
        _testDbContext.Dispose();
    }

    [Fact]
    public async Task GetAsync_ReturnsModel_WhenExists()
    {
        TestModel? testModel1 = await _testEntityRepository.GetAsync(1);
        TestModel? testModel2 = await _testEntityRepository.GetAsync(2);

        testModel1.Should().NotBeNull();
        testModel1.Name.Should().Be("Test1");

        testModel2.Should().NotBeNull();
        testModel2.Name.Should().Be("Test2");
    }

    [Fact]
    public async Task AllAsync_ReturnsAll()
    {
        IEnumerable<TestModel> testModels = await _testEntityRepository.AllAsync();

        testModels.Should().HaveCount(2);
    }

    [Fact]
    public async Task AddAsync_IsSuccessful_WhenValid()
    {
        TestModel testModel = await _testEntityRepository.AddAsync(
            new TestModel
            {
                Id = 0,
                Name = "Test3"
            });

        await _testDbContext.SaveChangesAsync();

        testModel.Should().NotBeNull();

        await using TestDbContext<TestModel> context = new(_dbContextOptions);

        context.Models.Should().HaveCount(3);

        TestModel? testModelFromContext = await context.Models.FindAsync(3);

        testModelFromContext.Should().NotBeNull();
        testModelFromContext.Name.Should().BeEquivalentTo("Test3");
    }

    [Fact]
    public async Task DeleteAsync_IsSuccessful_WhenValid()
    {
        await using TestDbContext<TestModel> context = new(_dbContextOptions);

        context.Models.Should().HaveCount(2);

        TestModel? testModelFromContext = await _testDbContext.Models.FindAsync(2);

        await _testEntityRepository.DeleteAsync(testModelFromContext!);
        await _testDbContext.SaveChangesAsync();

        context.Models.Should().HaveCountLessThan(2);
    }

    [Fact]
    public async Task UpdateAsync_IsSuccessful_WhenValid()
    {
        TestModel? testModelFromContext = await _testDbContext.Models.FindAsync(2);

        testModelFromContext!.Name = "NewName2";

        await _testEntityRepository.UpdateAsync(testModelFromContext!);
        await _testDbContext.SaveChangesAsync();

        await using TestDbContext<TestModel> context = new(_dbContextOptions);

        TestModel? updatedTestModelFromContext = await context.Models.FindAsync(2);

        updatedTestModelFromContext?.Name.Should().BeEquivalentTo("NewName2");
    }
}
