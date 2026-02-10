FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project files
COPY src/StaffPro.Domain/StaffPro.Domain.csproj src/StaffPro.Domain/
COPY src/StaffPro.Application/StaffPro.Application.csproj src/StaffPro.Application/
COPY src/StaffPro.Infrastructure/StaffPro.Infrastructure.csproj src/StaffPro.Infrastructure/
COPY src/StaffPro.Api/StaffPro.Api.csproj src/StaffPro.Api/

# Restore
RUN dotnet restore src/StaffPro.Api/StaffPro.Api.csproj

# Copy everything and build
COPY . .
RUN dotnet publish src/StaffPro.Api/StaffPro.Api.csproj -c Release -o /app/publish --no-restore

# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 5000
ENTRYPOINT ["dotnet", "StaffPro.Api.dll"]
