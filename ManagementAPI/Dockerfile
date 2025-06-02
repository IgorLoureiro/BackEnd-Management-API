FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /app

COPY . .

RUN dotnet restore

RUN dotnet publish -c Release -o /app/out

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

WORKDIR /app

RUN apt-get update && apt-get install -y bash netcat-openbsd && rm -rf /var/lib/apt/lists/*

COPY --from=build /app/out .

COPY templates ./templates

COPY wait-for.sh .

RUN chmod +x wait-for.sh

EXPOSE 80

ENTRYPOINT ["bash", "./wait-for.sh", "db", "3306", "dotnet", "ManagementAPI.dll"]
