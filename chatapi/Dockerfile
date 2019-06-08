FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["chatapi/chatapi.csproj", "chatapi/"]
RUN dotnet restore "chatapi/chatapi.csproj"
COPY . .
WORKDIR "/src/chatapi"
RUN dotnet build "chatapi.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "chatapi.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "chatapi.dll"]