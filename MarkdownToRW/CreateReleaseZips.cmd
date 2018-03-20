set /P version=Enter release number (e.g.: 1.28): 

:: create folder 
mkdir Release

(
echo namespace DragonMarkdown
echo {
echo     public static class DragonVersion
echo     {
echo         public static readonly decimal VERSION = %version%m;
echo     }
echo }
) > "%~dp0\MarkdownConverter\DragonVersion.cs"

:: GUI
:: Build Release in Visual Studio first
"C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\Common7\IDE\devenv.exe" "%~dp0\MarkdownToRW.sln" /Build Release
"%~dp0/7z/7za.exe" a -tzip "%~dp0\Release\MarkdownToRW_DotNetFramework_Mono_GUI_%version%.zip" "%~dp0\MarkdownToRW\bin\Release\*"

:: Portable
cd MarkdownToRWCore
call QuickPublish.cmd
call CreateReleases.cmd
cd ..

"%~dp0/7z/7za.exe" a -tzip "%~dp0\Release\MarkdownToRW_DotNetCore_Portable_%version%.zip" "%~dp0\MarkdownToRWCore\bin\Debug\netcoreapp2.0\publish\*"

:: Self Contained

"%~dp0/7z/7za.exe" a -tzip "%~dp0\Release\MarkdownToRW_DotNetCore_Windows_SelfContained_%version%.zip" "%~dp0\MarkdownToRWCore\bin\Release\netcoreapp2.0\win10-x64\publish\*"

"%~dp0/7z/7za.exe" a -tzip "%~dp0\Release\MarkdownToRW_DotNetCore_macOS_SelfContained_%version%.zip" "%~dp0\MarkdownToRWCore\bin\Release\netcoreapp2.0\osx-x64\publish\*"

"%~dp0/7z/7za.exe" a -tzip "%~dp0\Release\MarkdownToRW_DotNetCore_linux_SelfContained_%version%.zip" "%~dp0\MarkdownToRWCore\bin\Release\netcoreapp2.0\linux-x64\publish\*"