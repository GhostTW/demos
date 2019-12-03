FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS builder
WORKDIR /build
COPY ./ ./
RUN dotnet publish ./NetCore3Service.csproj -c Release -o ./output

FROM mcr.microsoft.com/dotnet/core/aspnet:3.0
WORKDIR /app
COPY --from=builder /build/output /usr/sbin/testapp
COPY ./testapp.service /etc/systemd/system/