dotnet build -c release
dotnet publish -c Release --framework netcoreapp2.0 -r win10-x64 --self-contained
dotnet publish -c Release --framework netcoreapp2.0 -r osx-x64 --self-contained
dotnet publish -c Release --framework netcoreapp2.0 -r linux-x64 --self-contained
dotnet publish -c Release --framework netcoreapp2.0