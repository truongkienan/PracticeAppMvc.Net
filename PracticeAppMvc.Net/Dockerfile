#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["PracticeAppMvc.Net/PracticeAppMvc.Net.csproj", "PracticeAppMvc.Net/"]
RUN dotnet restore "PracticeAppMvc.Net/PracticeAppMvc.Net.csproj"
COPY . .
WORKDIR "/src/PracticeAppMvc.Net"
RUN dotnet build "PracticeAppMvc.Net.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PracticeAppMvc.Net.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PracticeAppMvc.Net.dll"]