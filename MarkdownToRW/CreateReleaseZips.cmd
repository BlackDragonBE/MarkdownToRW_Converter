set /P version=Enter release number (e.g.: 1.28): 

REM create folder 
mkdir Release

REM GUI
REM Build Release in Visual Studio first
"%~dp0/7z/7za.exe" a -tzip "%~dp0\Release\MarkdownToRW_DotNetFramework_Mono_GUI_%version%.zip" "%~dp0\MarkdownToRW\bin\Release\*"

REM Portable
cd MarkdownToRWCore
call QuickPublish.cmd
call CreateReleases.cmd
cd ..

"%~dp0/7z/7za.exe" a -tzip "%~dp0\Release\MarkdownToRW_DotNetCore_Portable_%version%.zip" "%~dp0\MarkdownToRWCore\bin\Debug\netcoreapp2.0\publish\*"

REM Self Contained

"%~dp0/7z/7za.exe" a -tzip "%~dp0\Release\MarkdownToRW_DotNetCore_Windows_SelfContained_%version%.zip" "%~dp0\MarkdownToRWCore\bin\Release\netcoreapp2.0\win10-x64\publish\*"

"%~dp0/7z/7za.exe" a -tzip "%~dp0\Release\MarkdownToRW_DotNetCore_macOS_SelfContained_%version%.zip" "%~dp0\MarkdownToRWCore\bin\Release\netcoreapp2.0\osx-x64\publish\*"

"%~dp0/7z/7za.exe" a -tzip "%~dp0\Release\MarkdownToRW_DotNetCore_linux_SelfContained_%version%.zip" "%~dp0\MarkdownToRWCore\bin\Release\netcoreapp2.0\linux-x64\publish\*"