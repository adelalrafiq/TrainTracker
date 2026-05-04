# build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 As build
WORKDIR /src
COPY . .
RUN dotnet publish -c Release -o /app
 
# runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
 
ENTRYPOINT ["dotnet", "TrainTracker.Api.dll"]