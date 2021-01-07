#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
RUN apt-get update \
    && apt-get install -y wget \
    && wget https://github.com/wkhtmltopdf/packaging/releases/download/0.12.6-1/wkhtmltox_0.12.6-1.buster_amd64.deb \
    && dpkg -i ./wkhtmltox_0.12.6-1.buster_amd64.deb \
    ; apt-get -f install -y \
    && rm wkhtmltox_0.12.6-1.buster_amd64.deb \
    && rm -rf /var/cache/apt/lists/*




WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build


WORKDIR /src
COPY ["WkHtml2Image.Example.WebServer/WkHtml2Image.Example.WebServer.csproj", "WkHtml2Image.Example.WebServer/"]
RUN dotnet restore "WkHtml2Image.Example.WebServer/WkHtml2Image.Example.WebServer.csproj"
COPY . .

WORKDIR "/src/WkHtml2Image.Example.WebServer"
RUN dotnet build "WkHtml2Image.Example.WebServer.csproj" -c Release -o /app/build


FROM build AS publish
RUN dotnet publish "WkHtml2Image.Example.WebServer.csproj" -c Release -o /app/publish


FROM base AS final
WORKDIR /app 
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WkHtml2Image.Example.WebServer.dll"]