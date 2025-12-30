FROM mcr.microsoft.com/dotnet/sdk:8.0
WORKDIR /app
COPY . .
RUN dotnet publish TranslateAPI/TranslateAPI.AppHost/TranslateAPI.AppHost.csproj -c Release -o /app/publish
EXPOSE 5000
ENTRYPOINT ["dotnet", "/app/publish/TranslateAPI.AppHost.dll"]