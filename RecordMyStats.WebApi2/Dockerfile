#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["RecordMyStats.WebApi2/RecordMyStats.WebApi2.csproj", "RecordMyStats.WebApi2/"]
COPY ["RecordMyStats.Common/RecordMyStats.Common.csproj", "RecordMyStats.Common/"]
COPY ["RecordMyStats.DataAccess/RecordMyStats.DataAccess.csproj", "RecordMyStats.DataAccess/"]
RUN dotnet restore "RecordMyStats.WebApi2/RecordMyStats.WebApi2.csproj"
COPY . .
WORKDIR "/src/RecordMyStats.WebApi2"
RUN dotnet build "RecordMyStats.WebApi2.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RecordMyStats.WebApi2.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RecordMyStats.WebApi2.dll"]