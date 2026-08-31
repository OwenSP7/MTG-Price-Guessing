FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["MtgPriceGuesser.Client.csproj", "./"]
RUN dotnet restore "MtgPriceGuesser.Client.csproj"

COPY . .
RUN dotnet build "MtgPriceGuesser.Client.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MtgPriceGuesser.Client.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MtgPriceGuesser.Client.dll"]