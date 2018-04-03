dotnet build -c Release
dotnet publish -c Release --framework netcoreapp2.0 -r win-x64
dotnet publish -c Release --framework netcoreapp2.0 -r osx-x64
dotnet publish -c Release --framework netcoreapp2.0 -r linux-x64