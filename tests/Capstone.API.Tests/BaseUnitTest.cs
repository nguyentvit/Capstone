
using Capstone.API.Tests.Constants;
using Capstone.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Capstone.Api.Tests;

public abstract class BaseUnitTest
{
    //protected const string CURRENT_VERSION = "V1";
    private readonly string _baseAddress = "http://localhost/";
    private WebApplicationFactory<Program> _factory = default!;
    protected HttpClient HttpClient { get; private set; } = default!;
    protected ServiceProvider _serviceProvider = default!;
    // protected SqliteConnection _sqliteConnection = default!;
    private static async Task<Array?> InitDataToTableAsync(Type entityType, string filePath)
    {
        if (!File.Exists(filePath)) return null;

        var csvRecords = new List<object>();

        using var reader = new StreamReader(filePath);
        var headers = (await reader.ReadLineAsync())?.Split(',');

        while (!reader.EndOfStream)
        {
            var rows = (await reader.ReadLineAsync())?.Split(',');

            if (rows != null && headers != null)
            {
                var objectResult = new Dictionary<string, string>();

                for (var i = 0; i < headers.Length; i++)
                {
                    var key = headers[i].Replace("\"", string.Empty).Trim();

                    if (key == "Version") continue;

                    var value = rows[i]?.ToString()?.Replace("\"", string.Empty);

                    if (value == null) continue;
                    objectResult.TryAdd(key, value);
                }

                var jsonObjectResult = JsonConvert.SerializeObject(objectResult);
                var record = JsonConvert.DeserializeObject(jsonObjectResult, entityType);

                if (record != null) csvRecords.Add(record);
            }
        }

        var records = Array.CreateInstance(entityType, csvRecords.Count);
        csvRecords.ToArray().CopyTo(records, 0);

        return records;
    }
    // protected Task<int> ExecuteNonQueryAsync(string query, Dictionary<string, SqliteType>? parameters = null)
    // {
    //     var command = new SqliteCommand()
    //     {
    //         Connection = _sqliteConnection,
    //         CommandText = query
    //     };

    //     if (parameters != null)
    //     {
    //         foreach (var parameter in parameters)
    //         {
    //             command.Parameters.Add(parameter.Key, parameter.Value);
    //         }
    //     }

    //     return command.ExecuteNonQueryAsync();
    // }
    // protected async Task InitDbTestAsync(string dataDirPath, bool isEnsureCreated = true)
    // {
    //     if (string.IsNullOrWhiteSpace(dataDirPath)) throw new ArgumentNullException(nameof(dataDirPath));

    //     using var scope = _serviceProvider.CreateScope();
    //     var scopedServices = scope.ServiceProvider;
    //     var dbContext = scopedServices.GetRequiredService<ApplicationDbContext>();

    //     if (isEnsureCreated)
    //     {
    //         dbContext.Database.EnsureDeleted();
    //         dbContext.Database.EnsureCreated();
    //     }

    //     var entityTypes = dbContext.Model.GetEntityTypes().Select(t => t.ClrType).ToList();
    //     var dirPath = Path.Combine(ApiTestConst.BASE_DIRECTORY, dataDirPath);

    //     var fileList = Directory.GetFiles(dirPath);

    //     foreach (var file in fileList)
    //     {
    //         var fileName = Path.GetFileNameWithoutExtension(file);
    //         var entityType = entityTypes.FirstOrDefault(p => p.Name == fileName);

    //         if (entityType == null) continue;

    //         fileName = Path.Combine(dirPath, $"{fileName}.csv");

    //         var csvData = await InitDataToTableAsync(entityType, fileName);

    //         if (csvData?.Length > 0)
    //         {
    //             var dbSetMethod = dbContext.GetType().GetMethods().First(m => m.Name == "Set" && m.IsGenericMethod && m.GetParameters().Length == 0).MakeGenericMethod(entityType);
    //             var dbSet = dbSetMethod?.Invoke(dbContext, null);
    //             (dbSet?.GetType().GetMethod("AddRange", new[] { csvData.GetType() }))?.Invoke(dbSet, new object[] { csvData });
    //         }
    //     }

    //     dbContext.SaveChanges();

    // }
    protected static T? LoadExpectedDataFromFile<T>(params string[] paths) where T : class
    {
        var filePath = Path.Combine(ApiTestConst.BASE_DIRECTORY, Path.Combine(paths));

        using var streamReader = new StreamReader(filePath);
        using var jTextReader = new JsonTextReader(streamReader);
        {
            var serializer = new JsonSerializer();

            return serializer.Deserialize<T>(jTextReader);
        }
    }
    protected static object? LoadExpectedDataFromFile(Type type, params string[] paths)
    {
        var filePath = Path.Combine(ApiTestConst.BASE_DIRECTORY, Path.Combine(paths));

        using var streamReader = new StreamReader(filePath);
        using var jTextReader = new JsonTextReader(streamReader);
        {
            var serializer = new JsonSerializer();

            return serializer.Deserialize(jTextReader, type);
        }
    }
    [SetUp]
    public void Setup()
    {
        // _sqliteConnection = new SqliteConnection(ApiTestConst.DEFAULT_CONNECTION_STRING);
        // _sqliteConnection.Open();

        _factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                // var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
                // if (descriptor != null) services.Remove(descriptor);

                // var descriptorReadonly = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
                // if (descriptorReadonly != null) services.Remove(descriptorReadonly);

                // services.AddDbContext<ApplicationDbContext>(options =>
                // {
                //     options.UseSqlite(_sqliteConnection);
                // });

                // services.AddDbContext<ApplicationDbContext>(options =>
                // {
                //     options.UseSqlite(_sqliteConnection);
                // });

                services.AddSingleton<Microsoft.Extensions.Logging.ILoggerFactory, Microsoft.Extensions.Logging.Abstractions.NullLoggerFactory>();

                _serviceProvider = services.BuildServiceProvider();
            });
        });

        HttpClient = _factory.CreateClient();
        HttpClient.BaseAddress = new Uri(_baseAddress);
    }
    [TearDown]
    public void TearDown()
    {
        using (var scope = _serviceProvider?.CreateScope())
        {
            // var context = scope?.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            // context?.Database.EnsureDeleted();
        }

        // _sqliteConnection?.Close();
        // _sqliteConnection?.Dispose();
        // _serviceProvider?.Dispose();
        HttpClient?.Dispose();
        _factory?.Dispose();
    }
}