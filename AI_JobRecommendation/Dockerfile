#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["AI_JobRecommendation/AI_JobRecommendation.csproj", "AI_JobRecommendation/"]
RUN dotnet restore "AI_JobRecommendation/AI_JobRecommendation.csproj"
COPY . .
WORKDIR "/src/AI_JobRecommendation"
RUN dotnet build "AI_JobRecommendation.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AI_JobRecommendation.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AI_JobRecommendation.dll"]