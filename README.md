Criar migration:
dotnet ef migrations add CriacaoBanco  --project .\Infrastructure\Infrastructure.csproj --startup-project .\UsersAPI\UsersAPI.csproj

Executar a migration:
dotnet ef database update --project .\Infrastructure\Infrastructure.csproj --startup-project .\UsersAPI\UsersAPI.csproj