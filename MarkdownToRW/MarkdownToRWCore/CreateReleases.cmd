dotnet build -c Release
dotnet publish -c Release --framework netcoreapp2.0 -r win10-x64
dotnet publish -c Release --framework netcoreapp2.0 -r osx-x64
dotnet publish -c Release --framework netcoreapp2.0 -r linux-x64