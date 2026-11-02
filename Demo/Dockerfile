# ---- 构建阶段 ----
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore Demo.Api.Admin/Demo.Api.Admin.csproj
RUN dotnet publish Demo.Api.Admin/Demo.Api.Admin.csproj -c Release -o /app/publish

# ---- 运行阶段 ----
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
# 使用不含真实密钥的模板作为基础配置；连接串与 JWT 密钥由 docker-compose 环境变量覆盖
COPY Demo.Api.Admin/appsettings.example.json appsettings.json
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "Demo.Api.Admin.dll"]
