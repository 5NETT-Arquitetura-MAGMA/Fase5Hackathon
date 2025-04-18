using HealthMed.Migrator.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

internal class Program
{
    private static async Task<int> Main(string[] args)
    {
        try
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrEmpty(connectionString))
            {
                Console.WriteLine("String de conexão não configurada.");
                return 1;
            }

            var services = new ServiceCollection();
            services.AddDbContext<HealthMedDBContext>(options =>
                options.UseSqlServer(connectionString));

            var serviceProvider = services.BuildServiceProvider();

            using (var scope = serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<HealthMedDBContext>();
                //var migrator = scope.ServiceProvider.GetRequiredService<IMigrator>(); // Tenta obter o IMigrator

                //Console.WriteLine("IMigrator obtido com sucesso!");

                // Execute as operações de migração aqui
                await dbContext.Database.MigrateAsync();

                // Execute o seed do admin
                await HealthMedDBContext.SeedAdminUser(scope.ServiceProvider);
            }

            return 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ocorreu um erro: {ex}");
            return 1;
        }
    }
}