FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ./ ./
RUN dotnet restore
RUN dotnet publish -c release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
WORKDIR /app
ENV TZ=Asia/Jakarta
COPY --from=build /app .
ENTRYPOINT ["dotnet", "qcs_product.API.dll"]
