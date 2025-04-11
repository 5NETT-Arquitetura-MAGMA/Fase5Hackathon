using HealthMed.Migrator.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
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

            // Configurar o DbContext para injeção de dependência
            var services = new ServiceCollection()
                .AddDbContext<HealthMedDBContext>(options =>
                    options.UseSqlServer(connectionString)) // Substitua pelo seu provider
                .BuildServiceProvider();

            using var scope = services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<HealthMedDBContext>();
            var databaseCreator = dbContext.GetService<IDatabaseCreator>() as RelationalDatabaseCreator;
            var migrator = scope.ServiceProvider.GetRequiredService<IMigrator>();

            if (args.Length > 0)
            {
                string command = args[0].ToLower();

                switch (command)
                {
                    case "update":
                        string targetMigration = null;
                        if (args.Length > 1 && args[1] == "--target" && args.Length > 2)
                        {
                            targetMigration = args[2];
                            Console.WriteLine($"Aplicando migrações até: {targetMigration}");
                            await migrator.MigrateAsync(targetMigration);
                            Console.WriteLine("Migrações aplicadas com sucesso.");
                        }
                        else
                        {
                            Console.WriteLine("Aplicando as migrações pendentes...");
                            await migrator.MigrateAsync();
                            Console.WriteLine("Migrações aplicadas com sucesso.");
                        }
                        break;

                    default:
                        Console.WriteLine("Comando inválido.");
                        Console.WriteLine("Uso: MigrationConsole [add --name <Nome>|remove|update [--target <Nome>]|list]");
                        return 1;
                }
            }
            else
            {
                Console.WriteLine("Nenhum comando especificado.");
                Console.WriteLine("Uso: MigrationConsole [add --name <Nome>|remove|update [--target <Nome>]|list]");
                return 1;
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