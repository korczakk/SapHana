FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app

#Install Hana Client
COPY  hanaclient-2.11.20-linux-x64.tar.gz ./
RUN tar -xf  hanaclient-2.11.20-linux-x64.tar.gz
RUN ./client/hdbinst -p /app/sap
RUN rm -r sap/examples

COPY Program.cs ./
COPY SapHana.csproj ./
# Restore as distinct layers
RUN dotnet restore
# Build and publish a release
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build-env /app/out .
COPY --from=build-env /app/sap/dotnetcore ./sap/dotnetcore
ENV HDBDOTNETCORE=/app/sap/dotnetcore

ENTRYPOINT ["dotnet", "SapHana.dll"]
