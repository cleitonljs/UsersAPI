# Stage 1
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /app

COPY ["UsersAPI.csproj", "."]

RUN dotnet restore "./UsersAPI.csproj"

COPY . .

RUN dotnet publish -c Release -o /app/publish


# Stage 2
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080

ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "UsersAPI.dll"]

