FROM mcr.microsoft.com/dotnet/sdk:8.0 AS builder
WORKDIR /src
COPY TranslateAPI/TranslateAPI.AppHost TranslateAPI.AppHost
RUN cd TranslateAPI.AppHost && dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=builder /app/publish .
EXPOSE 5000
ENTRYPOINT ["dotnet", "TranslateAPI.AppHost.dll"]