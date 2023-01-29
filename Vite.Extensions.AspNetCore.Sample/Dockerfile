FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Vite.Extensions.AspNetCore.Sample/Vite.Extensions.AspNetCore.Sample.csproj", "Vite.Extensions.AspNetCore.Sample/"]
RUN dotnet restore "Vite.Extensions.AspNetCore.Sample/Vite.Extensions.AspNetCore.Sample.csproj"
COPY . .
WORKDIR "/src/Vite.Extensions.AspNetCore.Sample"
RUN dotnet build "Vite.Extensions.AspNetCore.Sample.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Vite.Extensions.AspNetCore.Sample.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Vite.Extensions.AspNetCore.Sample.dll"]
