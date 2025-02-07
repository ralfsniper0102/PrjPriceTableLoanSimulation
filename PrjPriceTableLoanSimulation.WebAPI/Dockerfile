FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

COPY ["PrjPriceTableLoanSimulation.WebAPI/PrjPriceTableLoanSimulation.WebAPI.csproj", "PrjPriceTableLoanSimulation.WebAPI/"]
COPY ["PrjPriceTableLoanSimulation.UseCase/PrjPriceTableLoanSimulation.UseCase.csproj", "PrjPriceTableLoanSimulation.UseCase/"]
COPY ["PrjPriceTableLoanSimulation.Domain/PrjPriceTableLoanSimulation.Domain.csproj", "PrjPriceTableLoanSimulation.Domain/"]
COPY ["PrjPriceTableLoanSimulation.Exception/PrjPriceTableLoanSimulation.Exception.csproj", "PrjPriceTableLoanSimulation.Exception/"]
COPY ["PrjPriceTableLoanSimulation.Localization/PrjPriceTableLoanSimulation.Localization.csproj", "PrjPriceTableLoanSimulation.Localization/"]
COPY ["PrjPriceTableLoanSimulation.Messaging/PrjPriceTableLoanSimulation.Messaging.csproj", "PrjPriceTableLoanSimulation.Messaging/"]
RUN dotnet restore "./PrjPriceTableLoanSimulation.WebAPI/PrjPriceTableLoanSimulation.WebAPI.csproj"

COPY . .
WORKDIR "/src/PrjPriceTableLoanSimulation.WebAPI"
RUN dotnet build "./PrjPriceTableLoanSimulation.WebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "./PrjPriceTableLoanSimulation.WebAPI.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
EXPOSE 5001
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PrjPriceTableLoanSimulation.WebAPI.dll"]