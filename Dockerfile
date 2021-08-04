# FROM mcr.microsoft.com/dotnet/aspnet:5.0
FROM mcr.microsoft.com/dotnet/sdk:5.0-focal

EXPOSE 5000/tcp
EXPOSE 8888/tcp
ENV ASPNETCORE_URLS http://*:5000

COPY bin/Release/net5.0/publish/ App/
WORKDIR /App
ENTRYPOINT ["dotnet", "ComplexWeb.dll"]
