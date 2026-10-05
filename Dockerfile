# syntax=docker/dockerfile:1
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY BirFikrimVar.sln ./
COPY src/BirFikrimVar/BirFikrimVar.csproj src/BirFikrimVar/
COPY src/BirFikrimVar.Business/BirFikrimVar.Business.csproj src/BirFikrimVar.Business/
COPY src/BirFikrimVar.DAL/BirFikrimVar.DAL.csproj src/BirFikrimVar.DAL/
COPY src/BirFikrimVar.Resources/BirFikrimVar.Resources.csproj src/BirFikrimVar.Resources/
RUN dotnet restore
COPY src ./src
RUN dotnet publish src/BirFikrimVar/BirFikrimVar.csproj -c Release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app ./
# Uploaded images live here (a named volume in docker-compose).
RUN mkdir -p /app/uploads && chown -R app:app /app/uploads
ENV Uploads__Path=/app/uploads \
    ASPNETCORE_ENVIRONMENT=Production \
    ASPNETCORE_URLS=http://0.0.0.0:8080
USER app
EXPOSE 8080
ENTRYPOINT ["dotnet", "BirFikrimVar.dll"]
