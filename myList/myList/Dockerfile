#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
RUN dotnet tool install --global dotnet-ef
ENV PATH="${PATH}:/root/.dotnet/tools"
WORKDIR "/src"

COPY ["myList/myList.csproj", "myList/"]
RUN dotnet restore "./myList/myList.csproj"

COPY . .
WORKDIR "/src/myList"
RUN dotnet build "./myList.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "./myList.csproj" -c Release -o /app/publish


FROM build AS final
WORKDIR /app
COPY --from=publish /app/publish .


