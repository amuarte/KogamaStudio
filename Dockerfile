FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY TranslateAPI/TranslateAPI.AppHost/bin/Release/net8.0/ .
EXPOSE 5000
CMD ["dotnet", "TranslateAPI.AppHost.dll"]