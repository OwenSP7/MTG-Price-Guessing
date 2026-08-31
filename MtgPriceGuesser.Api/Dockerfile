# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["MtgPriceGuesser.Api.csproj", "./"]
RUN dotnet restore "MtgPriceGuesser.Api.csproj"

COPY . .
RUN dotnet build "MtgPriceGuesser.Api.csproj" -c Release -o /app/build

# Stage 2: Publish
FROM build AS publish
RUN dotnet publish "MtgPriceGuesser.Api.csproj" -c Release -o /app/publish

# Stage 3: Run
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MtgPriceGuesser.Api.dll"]