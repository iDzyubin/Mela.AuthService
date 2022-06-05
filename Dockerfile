FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine AS build

WORKDIR src/Mela.AuthService.Api/

COPY src/Mela.AuthService.Api/*.csproj .
RUN dotnet restore Mela.AuthService.Api.csproj

COPY src/Mela.AuthService.Api .
RUN dotnet publish -c Release -o out \
                 --runtime alpine-x64 \
                 --self-contained true \
                 /p:PublishTrimmed=true \
                 /p:PublishSingleFile=true

FROM mcr.microsoft.com/dotnet/runtime-deps:6.0-alpine AS publish

WORKDIR /app
COPY --from=build /src/Mela.AuthService.Api/out .

EXPOSE 80
ENTRYPOINT ["./Mela.AuthService.Api"]