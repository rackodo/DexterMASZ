FROM mcr.microsoft.com/dotnet/nightly/sdk:7.0 AS build-env
WORKDIR /app

# Copy everything and build
COPY ./ ./
RUN dotnet publish -c Release -o ./bin

# Build runtime image
FROM mcr.microsoft.com/dotnet/nightly/aspnet:7.0
WORKDIR /app/bin
COPY --from=build-env /app/bin/ .

# Set timezone
ENV TZ=America/New_York
RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone

ENTRYPOINT ["dotnet", "Launch.dll"]
EXPOSE 80
