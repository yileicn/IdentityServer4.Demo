迁移
1.Identity
dotnet ef migrations add InitialIdentityServerPersistedGrantDbMigration -c PersistedGrantDbContext -o Data/Migrations/IdentityServer/PersistedGrantDb
dotnet ef migrations add InitialIdentityServerConfigurationDbMigration -c ConfigurationDbContext -o Data/Migrations/IdentityServer/ConfigurationDb

2.IdentityUser
dotnet ef migrations add InitialApplicationDbMigration -c ApplicationDbContext -o Data/Migrations/ApplicationDb