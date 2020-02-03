# Cheap Flights
Cheap Flights is a demo webapp with two main features:
1. Viewing all available flights from an internal database
2. Search for cheapest paths from any origin to destination

## Usage
```
cd src
dotnet restore
dotnet ef migrations add "Initial migration" # TODO
dotnet ef database update
dotnet run
```