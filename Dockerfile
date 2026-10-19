FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY src/ src/
RUN dotnet publish src/CicdLab.Api -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
ARG APP_VERSION=dev
ENV APP_VERSION=$APP_VERSION
ENV ASPNETCORE_HTTP_PORTS=8080
EXPOSE 5000
COPY --from=build /app .
USER $APP_UID
ENTRYPOINT ["dotnet", "CicdLab.Api.dll"]
