using FluentMigrator.Runner;
using rdlc.Migrations;

namespace rdlc.Extensions
{
    
    public static class MigrationManager
    {
        public static IHost MigrateDatabase(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var databaseService = scope.ServiceProvider.GetRequiredService<Database>();
                var migrationService = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();

                try
                {
                    databaseService.CreateDatabase("DapperMigrationExample");

                    migrationService.ListMigrations();
                    migrationService.MigrateUp();

                }
                catch
                {
                    //log errors or ...
                    throw;
                }
            }

            return host;
        }
    }
}
