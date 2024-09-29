#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Fall2024__SWD392_SE1704_111/Fall2024__SWD392_SE1704_111.csproj", "Fall2024__SWD392_SE1704_111/"]
RUN dotnet restore "./Fall2024__SWD392_SE1704_111/Fall2024__SWD392_SE1704_111.csproj"
COPY . .
WORKDIR "/src/Fall2024__SWD392_SE1704_111"
RUN dotnet build "./Fall2024__SWD392_SE1704_111.csproj" -c %BUILD_CONFIGURATION% -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Fall2024__SWD392_SE1704_111.csproj" -c %BUILD_CONFIGURATION% -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Fall2024__SWD392_SE1704_111.dll"]